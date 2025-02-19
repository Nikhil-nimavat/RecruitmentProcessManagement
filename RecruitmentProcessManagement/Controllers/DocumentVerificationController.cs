using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    [Authorize(Roles = "Admin, HR")]
    public class DocumentVerificationController : Controller
    {
        private readonly IDocumentVerificationService _documentVerificationService;

        public DocumentVerificationController(IDocumentVerificationService documentVerificationService)
        {
            _documentVerificationService = documentVerificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Verify(int candidateId, string status)
        {
            await _documentVerificationService.VerifyCandidateDocuments(candidateId, status, User.Identity.Name);

            TempData["SuccessMessage"] = "Documents verified successfully!";
            return RedirectToAction("HRDashboard");
        }
    }
}

