using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    [Authorize(Roles = "Admin, HR")]
    public class FinalSelectionController : Controller
    {
        private readonly IFinalSelectionService _finalSelectionService;
        private readonly UserManager<IdentityUser> _userManager;

        public FinalSelectionController(IFinalSelectionService finalSelectionService, UserManager<IdentityUser> userManager)
        {
            _finalSelectionService = finalSelectionService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SelectCandidate(int candidateId, int positionId, DateTime joiningDate)
        {
            string offerLetterPath = await _finalSelectionService.GenerateOfferLetter(candidateId, positionId, joiningDate);

            var candidate = await _finalSelectionService.GetCandidateById(candidateId);
            if (candidate == null)
            {
                TempData["ErrorMessage"] = "Candidate not found!";
                return RedirectToAction("HRDashboard");
            }

            var existingUser = await _userManager.FindByEmailAsync(candidate.Email);
            if (existingUser == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = candidate.Email,
                    Email = candidate.Email
                };

                // Assign default password Static for now then user can change it later on
                string defaultPassword = "Test#@123";
                var result = await _userManager.CreateAsync(newUser, defaultPassword);

                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Error adding candidate to system!";
                    return RedirectToAction("HRDashboard");
                }
            }

            await _finalSelectionService.MarkCandidateAsHired(candidateId, offerLetterPath);

            TempData["SuccessMessage"] = "Candidate selected and added to system!";
            return RedirectToAction("HRDashboard");
        }

        // Code without password logic 

        //public async Task<IActionResult> SelectCandidate(int candidateId, int positionId, DateTime joiningDate)
        //{
        //    string offerLetterPath = await _finalSelectionService.GenerateOfferLetter(candidateId, positionId, joiningDate);

        //    // Retrieve Candidate Details
        //    var candidate = await _finalSelectionService.GetCandidateById(candidateId);
        //    if (candidate == null)
        //    {
        //        TempData["ErrorMessage"] = "Candidate not found!";
        //        return RedirectToAction("HRDashboard");
        //    }

        //    var existingUser = await _userManager.FindByEmailAsync(candidate.Email);
        //    if (existingUser == null)
        //    {
        //        var newUser = new IdentityUser
        //        {
        //            UserName = candidate.Email,
        //            Email = candidate.Email
        //        };

        //        var result = await _userManager.CreateAsync(newUser);
        //        if (!result.Succeeded)
        //        {
        //            TempData["ErrorMessage"] = "Error adding candidate to system!";
        //            return RedirectToAction("HRDashboard");
        //        }
        //    }

        //    await _finalSelectionService.MarkCandidateAsHired(candidateId, offerLetterPath);

        //    TempData["SuccessMessage"] = "Candidate selected and added to system!";
        //    return RedirectToAction("HRDashboard");
        //}

    }
}
