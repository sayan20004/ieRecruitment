using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ieRecruitment.Models
{
    public class RecruitmentFormViewModel
    {
        [Display(Name = "Candidate Code")]
        public string? CandidateCode { get; set; } = "Candidate008735";

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; } = "BUSINESS DEVELOPMENT MANAGER (BDM) - KOLKATA";

        [Required(ErrorMessage = "Please upload your CV")]
        [Display(Name = "CV Upload")]
        public IFormFile? CVUpload { get; set; }

        [Required(ErrorMessage = "Please upload your recent photograph")]
        [Display(Name = "Photo Upload")]
        public IFormFile? PhotoUpload { get; set; }
    }
}
