using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class ReviewCandidateViewModel
    {
        public int CandidateId { get; set; }
        public List<SelectListItem> Candidates { get; set; } = new List<SelectListItem>();

        public int PositionId { get; set; }
        public List<SelectListItem> Positions { get; set; } = new List<SelectListItem>();

        public int ReviewerId { get; set; }

        public string Comments { get; set; }

        public string CurrentStatus { get; set; }
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "Selected", Text = "Selected" },
        new SelectListItem { Value = "Rejected", Text = "Rejected" },
        new SelectListItem { Value = "On Hold", Text = "On Hold" }
    };

        public List<SelectListItem> SkillsList { get; set; } = new List<SelectListItem>();
        public List<CandidateSkillViewModel> SelectedSkills { get; set; } = new List<CandidateSkillViewModel>();
    }

}
