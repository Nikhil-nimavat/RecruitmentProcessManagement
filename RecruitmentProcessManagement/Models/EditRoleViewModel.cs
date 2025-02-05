using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class EditRoleViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        public List<string>? Users { get; set; }
    }
}