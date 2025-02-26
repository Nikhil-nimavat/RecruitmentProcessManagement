using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class EditRoleViewModel
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string RoleName { get; set; }

        public List<string>? Users { get; set; }
    }
}