using System.ComponentModel.DataAnnotations;

namespace ieRecruitment.Models
{
    public class EducationalDetailsViewModel
    {
        // PhD
        [Display(Name = "PhD University")]
        public string? PhdUniversity { get; set; }
        
        [Display(Name = "PhD Department")]
        public string? PhdDepartment { get; set; }
        
        [Display(Name = "PhD Year of Award")]
        public string? PhdYear { get; set; }
        
        [Display(Name = "PhD Grade")]
        public string? PhdGrade { get; set; }

        // Masters
        [Display(Name = "Masters University")]
        public string? MastersUniversity { get; set; }
        
        [Display(Name = "Masters College/Institute")]
        public string? MastersCollege { get; set; }
        
        [Display(Name = "Masters Passing Year")]
        public string? MastersYear { get; set; }
        
        [Display(Name = "Masters Percentage/CGPA")]
        public string? MastersGrade { get; set; }

        // Bachelors
        [Display(Name = "Bachelors University")]
        public string? BachelorsUniversity { get; set; }
        
        [Display(Name = "Bachelors College/Institute")]
        public string? BachelorsCollege { get; set; }
        
        [Display(Name = "Bachelors Passing Year")]
        public string? BachelorsYear { get; set; }
        
        [Display(Name = "Bachelors Percentage/CGPA")]
        public string? BachelorsGrade { get; set; }

        // Diploma
        [Display(Name = "Diploma Institute/Board")]
        public string? DiplomaInstitute { get; set; }
        
        [Display(Name = "Diploma Passing Year")]
        public string? DiplomaYear { get; set; }
        
        [Display(Name = "Diploma Percentage/Grade")]
        public string? DiplomaGrade { get; set; }

        // Class 12th
        [Display(Name = "12th School/Board")]
        public string? TwelfthSchool { get; set; }
        
        [Display(Name = "12th Passing Year")]
        public string? TwelfthYear { get; set; }
        
        [Display(Name = "12th Percentage/Grade")]
        public string? TwelfthGrade { get; set; }

        // Class 10th
        [Display(Name = "10th School/Board")]
        public string? TenthSchool { get; set; }
        
        [Display(Name = "10th Passing Year")]
        public string? TenthYear { get; set; }
        
        [Display(Name = "10th Percentage/Grade")]
        public string? TenthGrade { get; set; }

        // Additional Qualifications (stored as JSON)
        public string? AdditionalQualificationsJSON { get; set; }

        // Project Details
        [Display(Name = "Vocational Training")]
        public string? VocationalTraining { get; set; }
        
        [Display(Name = "Project Name")]
        public string? ProjectName { get; set; }
        
        [Display(Name = "Project Tenure")]
        public string? ProjectTenure { get; set; }
        
        [Display(Name = "Project Description")]
        public string? ProjectDescription { get; set; }

        // Other Details
        [Display(Name = "Memberships")]
        public string? Memberships { get; set; }
        
        [Display(Name = "Other Qualification")]
        public string? OtherQualification { get; set; }
        
        [Display(Name = "Computer Proficiency")]
        public string? ComputerProficiency { get; set; }
        
        [Display(Name = "Other Training/Courses")]
        public string? OtherTraining { get; set; }
        
        [Display(Name = "Extra Curricular Activities")]
        public string? ExtraCurricular { get; set; }
    }

    public class AdditionalQualification
    {
        public string Institute { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
    }
}
