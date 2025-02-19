using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;
using System.Security.Claims;

namespace RecruitmentProcessManagement.Controllers
{
    public class CandidateDocumentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICandidateDocumentService _candidateDocumentService;
        private readonly ICandidateService _candidateService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CandidateDocumentController(
            IWebHostEnvironment webHostEnvironment,
            ICandidateDocumentService candidateDocumentService,
            ICandidateService candidateService,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _candidateDocumentService = candidateDocumentService;
            _context = context;
            _userManager = userManager;
            _candidateService = candidateService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer, HR")]
        public async Task<IActionResult> DocumentVerificationList()
        {
            var selectedCandidates = await _context.CandidateReviews
                .Where(cr => cr.Status == "Selected")
                .Include(cr => cr.Candidate)
                .Select(cr => cr.Candidate)
                .Distinct()
                .ToListAsync();

            return View(selectedCandidates);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer, HR")]
        public async Task<IActionResult> VerifyDocuments(int candidateId)
        {
            var candidate = await _context.Candidates
                .Include(c => c.CandidateDocuments)
                .FirstOrDefaultAsync(c => c.CandidateID == candidateId);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Reviewer, HR")]
        public async Task<IActionResult> Verify(int candidateId, string verificationStatus)
        {
            var candidate = await _context.Candidates.Include(c => c.CandidateDocuments).FirstOrDefaultAsync(c => c.CandidateID == candidateId);
            if (candidate == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var documentVerification = new DocumentVerification
            {
                CandidateID = candidateId,
                VerificationStatus = verificationStatus,
                VerificationDate = DateTime.Now,
                VerifiedBy = userId
            };

            _context.DocumentVerifications.Add(documentVerification);
            await _context.SaveChangesAsync();

            foreach (var doc in candidate.CandidateDocuments)
            {
                doc.VerificationStatus = verificationStatus;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document verification updated successfully!";
            return RedirectToAction("DocumentVerificationList");
        }

        [HttpGet]
        public IActionResult UploadDocuments()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile documentFile, string documentType)
        {
            var userEmail = User.Identity.Name;
            var candidate = await _candidateService.GetCandidateByEmail(userEmail);

            if (candidate == null)
            {
                TempData["ErrorMessage"] = "Candidate profile not found.";
                return RedirectToAction("UploadDocuments");
            }

            int candidateId = candidate.CandidateID;

            if (documentFile == null || documentFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a valid document.";
                return RedirectToAction("UploadDocuments");
            }

            var extension = System.IO.Path.GetExtension(documentFile.FileName).ToLower();

            //check for valid extension
            if (extension != ".pdf")
            {
                TempData["ErrorMessage"] = "Please upload a valid file. Only pdfs are allowed.";
                return RedirectToAction("UploadDocuments");
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
            return RedirectToAction("UploadDocuments");
        }

    }
}
