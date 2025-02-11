using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class FinalSelectionController : Controller
    {
        private readonly IFinalSelectionService _finalSelectionService;

        public FinalSelectionController(IFinalSelectionService finalSelectionService)
        {
            _finalSelectionService = finalSelectionService;
        }
        [HttpPost]
        public async Task<IActionResult> SelectCandidate(int candidateId, int positionId, DateTime joiningDate)
        {
            string offerLetterPath = await _finalSelectionService.GenerateOfferLetter(candidateId, positionId, joiningDate);

            TempData["SuccessMessage"] = "Candidate selected, and offer letter generated!";
            return RedirectToAction("HRDashboard");
        }
    }
}
