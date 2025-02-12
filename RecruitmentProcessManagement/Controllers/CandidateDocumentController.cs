using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class CandidateDocumentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICandidateDocumentService _candidateDocumentService;

        public CandidateDocumentController(IWebHostEnvironment webHostEnvironment, ICandidateDocumentService candidateDocumentService)
        {
            _webHostEnvironment = webHostEnvironment;
            _candidateDocumentService = candidateDocumentService;
        }

        [HttpGet]
        public IActionResult CandidateDashboard()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult HRDashboard()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(int candidateId, IFormFile documentFile, string documentType)
        {
            if (documentFile == null || documentFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a valid document.";
                return RedirectToAction("CandidateDashboard", new { candidateId });
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "documents");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{documentFile.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await documentFile.CopyToAsync(fileStream);
            }

            var candidateDocument = new CandidateDocument
            {
                CandidateID = candidateId,
                DocumentType = documentType,
                DocumentPath = "/uploads/documents/" + uniqueFileName,
                VerificationStatus = "Pending"
            };

            await _candidateDocumentService.AddDocument(candidateDocument);

            TempData["SuccessMessage"] = "Document uploaded successfully!";
            return RedirectToAction("CandidateDashboard", new { candidateId });
        }
    }
}
