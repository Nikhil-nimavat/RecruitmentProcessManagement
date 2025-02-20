﻿using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
