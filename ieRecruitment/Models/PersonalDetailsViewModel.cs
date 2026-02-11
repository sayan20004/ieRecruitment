using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class PersonalDetailsViewModel
    {
        // Current Address
        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string CurrentAddressLine1 { get; set; } = string.Empty;

        [Display(Name = "Address Line 2")]
        public string? CurrentAddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string CurrentCity { get; set; } = string.Empty;

        [Display(Name = "Post Office")]
        public string? CurrentPostOffice { get; set; }

        [Required(ErrorMessage = "Pin Code is required")]
        [Display(Name = "Pin Code")]
        public string CurrentPinCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        public string CurrentDistrict { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        public string CurrentState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public string CurrentCountry { get; set; } = string.Empty;

        // Permanent Address
        public bool SameAsCurrent { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string PermanentAddressLine1 { get; set; } = string.Empty;

        [Display(Name = "Address Line 2")]
        public string? PermanentAddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string PermanentCity { get; set; } = string.Empty;

        [Display(Name = "Post Office")]
        public string? PermanentPostOffice { get; set; }

        [Required(ErrorMessage = "Pin Code is required")]
        [Display(Name = "Pin Code")]
        public string PermanentPinCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        public string PermanentDistrict { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        public string PermanentState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        public string PermanentCountry { get; set; } = string.Empty;

        // Other Details
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nationality is required")]
        public string Nationality { get; set; } = string.Empty;

        [Required(ErrorMessage = "Marital Status is required")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Religion is required")]
        public string Religion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please specify if willing to relocate")]
        [Display(Name = "Willing to Relocate")]
        public string WillingToRelocate { get; set; } = string.Empty;

        [Display(Name = "Social Media Platform")]
        public string? SocialMediaPlatform { get; set; }

        [Display(Name = "Profile Link")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? ProfileLink { get; set; }
    }
}
