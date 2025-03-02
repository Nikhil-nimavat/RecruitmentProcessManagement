﻿using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class PositionSkill
    {
        [Key]
        public int PositionSkillID { get; set; }
        public int PositionID { get; set; }
        public int SkillID { get; set; }

        // Navigation properties
        public Position Position { get; set; }
        public Skill Skill { get; set; }
    }
}
