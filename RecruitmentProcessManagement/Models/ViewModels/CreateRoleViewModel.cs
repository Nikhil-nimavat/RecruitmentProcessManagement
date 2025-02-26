using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public required string RoleName { get; set; }
    }
}
