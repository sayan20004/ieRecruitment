using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class FamilyDetailsViewModel
    {
        // Store as JSON strings for simplicity
        public string? FamilyMembersJSON { get; set; }
        public string? EmergencyContactsJSON { get; set; }
        public string? LanguagesJSON { get; set; }
    }

    // Helper classes for client-side JSON structure
    public class FamilyMember
    {
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
    }

    public class EmergencyContact
    {
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }

    public class Language
    {
        public string Name { get; set; } = string.Empty;
        public string CanRead { get; set; } = string.Empty;
        public string CanWrite { get; set; } = string.Empty;
        public string CanSpeak { get; set; } = string.Empty;
    }
}
