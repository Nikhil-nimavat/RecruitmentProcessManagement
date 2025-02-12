using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class DocumentVerificationController : Controller
    {
        private readonly IDocumentVerificationService _documentVerificationService;

        public DocumentVerificationController(IDocumentVerificationService documentVerificationService)
        {
            _documentVerificationService = documentVerificationService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Verify(int candidateId, string status)
        {
            await _documentVerificationService.VerifyCandidateDocuments(candidateId, status, User.Identity.Name);

            TempData["SuccessMessage"] = "Documents verified successfully!";
            return RedirectToAction("HRDashboard");
        }
    }
}

