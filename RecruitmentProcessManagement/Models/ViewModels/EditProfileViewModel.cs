using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{ 
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        [Display(Name = "User Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string? UserName { get; set; }

        [Display(Name = "Email Address")]
        public string? Email { get; set; } 
    }
}
