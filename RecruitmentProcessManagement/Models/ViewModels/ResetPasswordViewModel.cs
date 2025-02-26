using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }

    }
}