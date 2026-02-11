using System.Text.Json.Serialization;

namespace ieRecruitment.Models
{
    /// <summary>
    /// DTO representing structured data extracted from a CV/Resume via Gemini API.
    /// Field names use camelCase JSON to match the JavaScript auto-fill logic.
    /// </summary>
    public class CvExtractedData
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("professionalSummary")]
        public string? ProfessionalSummary { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("nationality")]
        public string? Nationality { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string? DateOfBirth { get; set; }

        [JsonPropertyName("maritalStatus")]
        public string? MaritalStatus { get; set; }

        [JsonPropertyName("currentAddress")]
        public ExtractedAddress? CurrentAddress { get; set; }

        [JsonPropertyName("socialMediaPlatform")]
        public string? SocialMediaPlatform { get; set; }

        [JsonPropertyName("profileLink")]
        public string? ProfileLink { get; set; }

        [JsonPropertyName("education")]
        public ExtractedEducation? Education { get; set; }

        [JsonPropertyName("workExperience")]
        public ExtractedWorkExperience? WorkExperience { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("religion")]
        public string? Religion { get; set; }

        [JsonPropertyName("willingToRelocate")]
        public string? WillingToRelocate { get; set; }

        [JsonPropertyName("noticePeriod")]
        public string? NoticePeriod { get; set; }

        [JsonPropertyName("skills")]
        public List<string>? Skills { get; set; }

        [JsonPropertyName("languages")]
        public List<ExtractedLanguage>? Languages { get; set; }

        [JsonPropertyName("certifications")]
        public string? Certifications { get; set; }

        [JsonPropertyName("memberships")]
        public string? Memberships { get; set; }

        [JsonPropertyName("vocationalTraining")]
        public string? VocationalTraining { get; set; }

        [JsonPropertyName("extraCurricular")]
        public string? ExtraCurricular { get; set; }

        [JsonPropertyName("whySuitable")]
        public string? WhySuitable { get; set; }

        [JsonPropertyName("internships")]
        public List<ExtractedInternship>? Internships { get; set; }

        [JsonPropertyName("references")]
        public List<ExtractedReference>? References { get; set; }
    }

    public class ExtractedReference
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("relation")]
        public string? Relation { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
    }

    public class ExtractedInternship
    {
        [JsonPropertyName("company")]
        public string? Company { get; set; }

        [JsonPropertyName("position")]
        public string? Position { get; set; }

        [JsonPropertyName("startDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class ExtractedAddress
    {
        [JsonPropertyName("addressLine1")]
        public string? AddressLine1 { get; set; }

        [JsonPropertyName("addressLine2")]
        public string? AddressLine2 { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("pinCode")]
        public string? PinCode { get; set; }

        [JsonPropertyName("district")]
        public string? District { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }
    }

    public class ExtractedEducation
    {
        [JsonPropertyName("phd")]
        public ExtractedDegree? Phd { get; set; }

        [JsonPropertyName("masters")]
        public ExtractedDegree? Masters { get; set; }

        [JsonPropertyName("bachelors")]
        public ExtractedDegree? Bachelors { get; set; }

        [JsonPropertyName("diploma")]
        public ExtractedDegree? Diploma { get; set; }

        [JsonPropertyName("twelfth")]
        public ExtractedSchool? Twelfth { get; set; }

        [JsonPropertyName("tenth")]
        public ExtractedSchool? Tenth { get; set; }

        [JsonPropertyName("computerProficiency")]
        public string? ComputerProficiency { get; set; }

        [JsonPropertyName("projects")]
        public List<ExtractedProject>? Projects { get; set; }
    }

    public class ExtractedDegree
    {
        [JsonPropertyName("university")]
        public string? University { get; set; }

        [JsonPropertyName("college")]
        public string? College { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; }

        [JsonPropertyName("year")]
        public string? Year { get; set; }

        [JsonPropertyName("grade")]
        public string? Grade { get; set; }
    }

    public class ExtractedSchool
    {
        [JsonPropertyName("school")]
        public string? School { get; set; }

        [JsonPropertyName("year")]
        public string? Year { get; set; }

        [JsonPropertyName("grade")]
        public string? Grade { get; set; }
    }

    public class ExtractedProject
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tenure")]
        public string? Tenure { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class ExtractedWorkExperience
    {
        [JsonPropertyName("experienceType")]
        public string? ExperienceType { get; set; }

        [JsonPropertyName("experiences")]
        public List<ExtractedExperienceEntry>? Experiences { get; set; }
    }

    public class ExtractedExperienceEntry
    {
        [JsonPropertyName("company")]
        public string? Company { get; set; }

        [JsonPropertyName("position")]
        public string? Position { get; set; }

        [JsonPropertyName("startDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class ExtractedLanguage
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("canRead")]
        public string? CanRead { get; set; }

        [JsonPropertyName("canWrite")]
        public string? CanWrite { get; set; }

        [JsonPropertyName("canSpeak")]
        public string? CanSpeak { get; set; }
    }
}
