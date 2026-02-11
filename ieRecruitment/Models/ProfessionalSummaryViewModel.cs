using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class ProfessionalSummaryViewModel
    {
        [Required(ErrorMessage = "Professional summary is required")]
        [StringLength(5000, ErrorMessage = "Professional summary cannot exceed 5000 characters")]
        [Display(Name = "Professional Summary")]
        public string? Summary { get; set; }
    }
}
