using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class OtherDetailsViewModel
    {
        [Required(ErrorMessage = "Please explain why you are suitable for this position")]
        [StringLength(2000, ErrorMessage = "Maximum 2000 characters allowed")]
        [Display(Name = "Explain Why Are You Suitable For This Position")]
        public string? WhySuitable { get; set; }

        [Display(Name = "Who Referred You")]
        public string? Referrer { get; set; }

        [Required(ErrorMessage = "Aadhaar number is required")]
        [RegularExpression(@"^\d{4}-?\d{4}-?\d{4}$", ErrorMessage = "Invalid Aadhaar number format")]
        [Display(Name = "Aadhaar No.")]
        public string? AadhaarNo { get; set; }

        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [Display(Name = "PAN Card No.")]
        public string? PanNo { get; set; }

        [Display(Name = "UAN No.")]
        public string? UanNo { get; set; }

        [Display(Name = "I don't have UAN No.")]
        public bool NoUan { get; set; }

        [Display(Name = "ESIC No.")]
        public string? EsicNo { get; set; }

        [Display(Name = "I don't have ESIC No.")]
        public bool NoEsic { get; set; }

        [Display(Name = "Interviewed before?")]
        public bool? InterviewedBefore { get; set; }

        [Display(Name = "Any relatives working here?")]
        public bool? RelativesWorking { get; set; }

        [Display(Name = "Any disability/illness?")]
        public bool? HasDisability { get; set; }

        [Display(Name = "Ever dismissed from company?")]
        public bool? EverDismissed { get; set; }

        [Display(Name = "Ever convicted?")]
        public bool? EverConvicted { get; set; }

        [Required(ErrorMessage = "Joining time is required")]
        [Range(1, 365, ErrorMessage = "Joining time must be between 1 and 365 days")]
        [Display(Name = "Joining time (Days)")]
        public int? JoiningTimeDays { get; set; }

        // References stored as JSON
        [Display(Name = "References")]
        public string? ReferencesJSON { get; set; }
    }

    public class ReferenceItem
    {
        public string? Name { get; set; }
        public string? Relation { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
    }
}
