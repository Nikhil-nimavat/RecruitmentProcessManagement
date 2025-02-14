using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class CandidateReviewSelectionViewModel
    {
        public int ReviewerID { get; set; }
        public int SelectedCandidateID { get; set; }
        public int SelectedPositionID { get; set; }

        public List<SelectListItem> Candidates { get; set; }
        public List<SelectListItem> Positions { get; set; }
    }

}
