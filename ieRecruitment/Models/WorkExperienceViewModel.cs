using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class WorkExperienceViewModel
    {
        [Required(ErrorMessage = "Please select your experience type")]
        public string ExperienceType { get; set; } = "Fresher";

        // Fresher fields
        public bool HasInternship { get; set; }
        public string? InternshipsJSON { get; set; }

        // Experienced fields
        public string? EmploymentDetailsJSON { get; set; }

        // Sales Experience fields
        public string? SalesExperienceJSON { get; set; }
    }

    public class InternshipDetails
    {
        public string? Company { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Compensation { get; set; }
        public string? CertificateFileName { get; set; }
        public string? Description { get; set; }
    }
}
