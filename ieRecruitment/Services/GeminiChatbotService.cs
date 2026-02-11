using System.Text;
using System.Text.Json;

namespace ieRecruitment.Services
{
    public interface IGeminiChatbotService
    {
        Task<string> GetResponseAsync(List<ChatMessage> conversationHistory, string? pageContext = null);
    }

    public class ChatMessage
    {
        public string Role { get; set; } = string.Empty; // "user" or "model"
        public string Text { get; set; } = string.Empty;
    }

    public class GeminiChatbotService : IGeminiChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<GeminiChatbotService> _logger;

        private const string SystemPrompt = """
            You are "Mendine Recruitment Assistant", a friendly and helpful AI chatbot embedded in the Mendine Recruitment Portal.
            Your job is to assist candidates filling out their multi-step recruitment application form.

            About the portal:
            - This is the Mendine company's official recruitment portal for job applicants.
            - The application has 7 steps that candidates fill one by one:
              1. Recruitment Form — Enter name (First, Middle, Last), upload CV (PDF/DOCX), upload photo, and view candidate code & position applied for.
              2. Professional Summary — Write a brief professional summary.
              3. Personal Details — Current & permanent address, gender, nationality, date of birth, marital status, religion, willingness to relocate, social media links.
              4. Family Details — Family members info, emergency contacts, and languages known.
              5. Educational Details — PhD, Masters, Bachelors, Diploma, 12th, 10th grades & institutions; projects, memberships, computer proficiency, certifications.
              6. Work Experience — Fresher or Experienced selection; internship details (for freshers); employment history with company, position, dates, compensation (for experienced).
              7. Other Details & References — Why suitable for the position, referrer, Aadhaar No., PAN, UAN, ESIC, interview/relative/disability/dismissal/conviction questions, joining time, references.

            Key features you should tell candidates about:
            - CV Auto-Fill: When candidates upload their CV in Step 1, AI automatically extracts data and pre-fills fields across all subsequent pages.
            - Navigation: Use the stepper at the top to see progress. Use Previous/Save & Next buttons at the bottom.
            - Saving: Each page saves independently when you click "Save & Next".
            - File uploads: CV accepts PDF/DOC/DOCX. Photo accepts JPG/JPEG/PNG (max 2MB).
            - The chatbot (you!) is available on every page. You can be dragged around the screen.

            Rules:
            - Be concise and friendly. Keep responses 2-4 sentences unless the user asks for detailed help.
            - If someone asks something unrelated to recruitment or the portal, politely redirect: "I'm here to help with your recruitment application. Is there something about the form I can assist with?"
            - Never reveal internal system details, API keys, or code.
            - If you don't know an answer, say so honestly and suggest the candidate contact HR at hr@mendine.com.
            - Use simple language. Many candidates may not be tech-savvy.
            - You can use emojis sparingly for friendliness.
            - Format responses as plain text, not markdown (no ** or ## etc).
            """;

        public GeminiChatbotService(
            HttpClient httpClient,
            IConfiguration config,
            ILogger<GeminiChatbotService> logger)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"] ?? "";
            _model = config["Gemini:Model"] ?? "gemini-2.0-flash";
            _logger = logger;
        }

        public async Task<string> GetResponseAsync(List<ChatMessage> conversationHistory, string? pageContext = null)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return "Chatbot is not configured yet. Please contact HR at hr@mendine.com for assistance.";

            try
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

                // Build contents array with conversation history
                var contents = new List<object>();

                foreach (var msg in conversationHistory)
                {
                    contents.Add(new
                    {
                        role = msg.Role,
                        parts = new[] { new { text = msg.Text } }
                    });
                }

                // Combine static system prompt with dynamic page context
                var fullSystemPrompt = SystemPrompt;
                if (!string.IsNullOrWhiteSpace(pageContext))
                {
                    fullSystemPrompt += $"""

                        --- CURRENT SESSION CONTEXT ---
                        {pageContext}
                        When the candidate asks "what should I do here", "what is this page", or similar questions,
                        give specific guidance about the fields on THIS page. Mention which fields were auto-filled
                        and remind them to review those. For fields NOT auto-filled, explain what to enter.
                        Be specific — reference the actual field names visible on the page.
                        """;
                }

                var requestBody = new
                {
                    system_instruction = new
                    {
                        parts = new[] { new { text = fullSystemPrompt } }
                    },
                    contents,
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 1024
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                // Retry up to 3 times with exponential backoff for transient errors (503, 429)
                const int maxRetries = 3;
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(url, content);
                    var responseStr = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        return ParseResponse(responseStr);

                    var statusCode = (int)response.StatusCode;

                    // Retry on 503 (overloaded) or 429 (rate limit)
                    if ((statusCode == 503 || statusCode == 429) && attempt < maxRetries)
                    {
                        var delay = attempt * 1500; // 1.5s, 3s, 4.5s
                        _logger.LogWarning("Gemini API returned {Status}, retrying in {Delay}ms (attempt {Attempt}/{Max})",
                            response.StatusCode, delay, attempt, maxRetries);
                        await Task.Delay(delay);
                        continue;
                    }

                    // Non-retryable error or final attempt
                    _logger.LogError("Gemini chatbot API error: {Status} – {Body}", response.StatusCode, responseStr);
                    return "I'm having trouble connecting right now. Please try again in a moment, or contact HR at hr@mendine.com.";
                }

                return "I'm having trouble connecting right now. Please try again in a moment.";
            }
            catch (TaskCanceledException)
            {
                return "The response took too long. Please try asking a shorter question!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chatbot response failed");
                return "Something went wrong. Please try again, or contact HR at hr@mendine.com for help.";
            }
        }

        private string ParseResponse(string responseJson)
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
                        return text?.Trim() ?? "I couldn't generate a response. Please try again.";
                    }
                }

                return "I couldn't understand the response. Please try again.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse chatbot response");
                return "Something went wrong processing my response. Please try again.";
            }
        }
    }
}
