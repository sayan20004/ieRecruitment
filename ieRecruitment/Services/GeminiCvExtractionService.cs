using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using ieRecruitment.Models;

namespace ieRecruitment.Services
{
    public interface IGeminiCvService
    {
        Task<CvExtractedData?> ExtractFromCvAsync(IFormFile file);
    }

    public class GeminiCvExtractionService : IGeminiCvService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<GeminiCvExtractionService> _logger;

        public GeminiCvExtractionService(
            HttpClient httpClient,
            IConfiguration config,
            ILogger<GeminiCvExtractionService> logger)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"] ?? "";
            _model = config["Gemini:Model"] ?? "gemini-2.0-flash";
            _logger = logger;
        }

        public async Task<CvExtractedData?> ExtractFromCvAsync(IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Gemini API key is not configured. Add Gemini:ApiKey to appsettings.json.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            object requestBody = ext switch
            {
                ".pdf" => await BuildPdfRequestAsync(file),
                ".docx" => await BuildDocxRequestAsync(file),
                ".doc" => await BuildPdfRequestAsync(file), // try as binary; recommend PDF
                _ => throw new ArgumentException($"Unsupported file format: {ext}. Please upload a PDF or DOCX file.")
            };

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Calling Gemini API for CV extraction ({Ext}, {Size} bytes)", ext, file.Length);

            var response = await _httpClient.PostAsync(url, content);
            var responseStr = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API error: {Status} – {Body}", response.StatusCode, responseStr);
                throw new Exception($"Gemini API returned {(int)response.StatusCode}. Please check your API key and try again.");
            }

            return ParseGeminiResponse(responseStr);
        }

        // ── PDF: send as native inline_data ──────────────────────────────
        private async Task<object> BuildPdfRequestAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { inline_data = new { mime_type = "application/pdf", data = base64 } },
                            new { text = GetExtractionPrompt() }
                        }
                    }
                },
                generationConfig = new { response_mime_type = "application/json", temperature = 0.1 }
            };
        }

        // ── DOCX: extract text via ZIP/XML, send as text prompt ──────────
        private async Task<object> BuildDocxRequestAsync(IFormFile file)
        {
            var text = await ExtractTextFromDocxAsync(file);

            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Could not extract text from the DOCX file. Please upload a PDF instead.");

            return new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new object[]
                        {
                            new { text = $"Here is the text content extracted from a CV/Resume:\n\n---\n{text}\n---\n\n{GetExtractionPrompt()}" }
                        }
                    }
                },
                generationConfig = new { response_mime_type = "application/json", temperature = 0.1 }
            };
        }

        // ── DOCX text extractor (no external packages) ──────────────────
        private static async Task<string> ExtractTextFromDocxAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            using var zip = new ZipArchive(ms, ZipArchiveMode.Read);
            var entry = zip.GetEntry("word/document.xml");
            if (entry == null) return string.Empty;

            await using var entryStream = entry.Open();
            var doc = await XDocument.LoadAsync(entryStream, LoadOptions.None, CancellationToken.None);
            var ns = XNamespace.Get("http://schemas.openxmlformats.org/wordprocessingml/2006/main");

            var sb = new StringBuilder();
            foreach (var para in doc.Descendants(ns + "p"))
            {
                var line = string.Join("", para.Descendants(ns + "t").Select(t => t.Value));
                if (!string.IsNullOrWhiteSpace(line))
                    sb.AppendLine(line);
            }

            return sb.ToString();
        }

        // ── Extraction prompt ────────────────────────────────────────────
        private static string GetExtractionPrompt()
        {
            return """
            You are a CV/Resume parser. Extract structured information from the provided CV and return ONLY valid JSON.
            If a field cannot be determined from the CV, set it to null. Do not guess or fabricate data.

            Return JSON in this exact structure:
            {
              "firstName": "string or null",
              "middleName": "string or null",
              "lastName": "string or null",
              "phone": "phone number string or null",
              "email": "email address string or null",
              "professionalSummary": "A concise 2-4 sentence professional summary based on the CV, or null",
              "gender": "Male or Female or Other or null",
              "nationality": "string or null",
              "dateOfBirth": "YYYY-MM-DD format or null",
              "maritalStatus": "Single or Married or Divorced or Widowed or null",
              "religion": "Hindu or Muslim or Christian or Sikh or Buddhist or Jain or Other or null",
              "willingToRelocate": "Yes or No or null",
              "currentAddress": {
                "addressLine1": "string or null",
                "addressLine2": "string or null",
                "city": "string or null",
                "pinCode": "string or null",
                "district": "string or null",
                "state": "string or null",
                "country": "string or null"
              },
              "socialMediaPlatform": "LinkedIn or GitHub or Other or null",
              "profileLink": "URL string or null",
              "education": {
                "phd": { "university": "string or null", "department": "string or null", "college": "string or null", "year": "passing year string or null", "grade": "percentage/CGPA string or null" },
                "masters": { "university": "string or null", "department": null, "college": "string or null", "year": "string or null", "grade": "string or null" },
                "bachelors": { "university": "string or null", "department": null, "college": "string or null", "year": "string or null", "grade": "string or null" },
                "diploma": { "university": null, "department": null, "college": "institute name or null", "year": "string or null", "grade": "string or null" },
                "twelfth": { "school": "school/board name or null", "year": "string or null", "grade": "percentage or null" },
                "tenth": { "school": "school/board name or null", "year": "string or null", "grade": "percentage or null" },
                "computerProficiency": "description of computer skills or null",
                "projects": [{ "name": "string", "tenure": "duration string", "description": "string" }]
              },
              "workExperience": {
                "experienceType": "Fresher or Experienced",
                "experiences": [{ "company": "string", "position": "string", "startDate": "YYYY-MM-DD or null", "endDate": "YYYY-MM-DD or Present or null", "description": "brief role description" }]
              },
              "internships": [{ "company": "string", "position": "string", "startDate": "YYYY-MM-DD or null", "endDate": "YYYY-MM-DD or null", "description": "brief description or null" }],
              "skills": ["skill1", "skill2"],
              "languages": [{ "name": "language name", "canRead": "Yes or No", "canWrite": "Yes or No", "canSpeak": "Yes or No" }],
              "certifications": "comma-separated list of certifications/courses, or null",
              "memberships": "comma-separated list of professional memberships/societies, or null",
              "vocationalTraining": "vocational training details or null",
              "extraCurricular": "extra-curricular activities or hobbies, or null",
              "noticePeriod": "number of days as string (e.g. '30'), or null",
              "whySuitable": "ALWAYS generate a 2-3 sentence explanation of why this candidate is suitable based on their skills, experience and education.",
              "references": [{ "name": "reference person's full name", "relation": "e.g. Manager, Professor, Colleague", "address": "address or organization", "phone": "phone number or null" }]
            }

            Important rules:
            - For name, split the full name into firstName, middleName, lastName correctly.
            - For phone, extract any mobile/phone number found in the CV.
            - For email, extract any email address found in the CV.
            - For education, fill only the levels that are mentioned in the CV.
            - For workExperience, set experienceType to "Fresher" if no full-time work experience is listed.
            - For internships, extract any internship entries separately from full-time work experience.
            - For languages: ALWAYS return at least one language. If the CV has a "Languages" section, extract all listed languages. If not, infer the language the CV is written in (e.g. if written in English, return [{"name": "English", "canRead": "Yes", "canWrite": "Yes", "canSpeak": "Yes"}]). The language name MUST be one of: English, Bengali, Hindi, Tamil, Telugu, Marathi, Gujarati, Kannada, Malayalam, Punjabi, Urdu. If the language doesn't match any of these, use the closest match or skip it.
            - For certifications, include any courses, certifications, or training mentioned.
            - For noticePeriod, extract if notice period or joining time is mentioned.
            - For whySuitable: ALWAYS generate this field. Write 2-3 sentences summarising why this candidate would be a good hire based on their skills, experience, and qualifications.
            - For references, extract if the CV has a "References" section with contact persons. If not present, return an empty array [].
            - Return only the JSON object, no explanation text.
            """;
        }

        // ── Parse Gemini API response ────────────────────────────────────
        private CvExtractedData? ParseGeminiResponse(string responseJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;

                if (root.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var candidate = candidates[0];
                    if (candidate.TryGetProperty("content", out var content) &&
                        content.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            // Strip markdown code fences if present
                            text = text.Trim();
                            if (text.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                                text = text[7..];
                            else if (text.StartsWith("```"))
                                text = text[3..];
                            if (text.EndsWith("```"))
                                text = text[..^3];
                            text = text.Trim();

                            _logger.LogInformation("Gemini extraction successful ({Length} chars)", text.Length);
                            return JsonSerializer.Deserialize<CvExtractedData>(text);
                        }
                    }
                }

                _logger.LogWarning("Unexpected Gemini response structure");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Gemini response");
                return null;
            }
        }
    }
}
