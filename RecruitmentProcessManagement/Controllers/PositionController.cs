using System;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Controllers;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace RecruitmentProcessManagement.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICandidateService _candidateservice;

        public PositionController(IPositionService positionService,
            ApplicationDbContext context, IServiceProvider serviceProvider,
            IWebHostEnvironment webHostEnvironment, ICandidateService candidateservice)
        {
            _positionService = positionService;
            _context = context;
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
            _candidateservice = candidateservice;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Candidates = await _context.Candidates.ToListAsync();
            return View(positions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Create(Position position)
        {
            if (ModelState.IsValid)
            {
                await _positionService.AddPositionAsync(position);
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Edit(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Edit(int id, Position position)
        {
            if (id != position.PositionID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _positionService.UpdatePositionAsync(position);
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Delete(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> Delete(Position position)
        {
            await _positionService.DeletePositionAsync(position.PositionID);
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public async Task<IActionResult> Close(int id, string ReasonForClosure)
        //{
        //    var position = await _positionService.GetPositionByIdAsync(id);
        //    if (position == null)
        //    {
        //        return NotFound();
        //    }
                
        //    position.Status = "Closed";
        //    position.ReasonForClosure = ReasonForClosure;
        //    await _positionService.UpdatePositionAsync(position);

        //    return RedirectToAction(nameof(Index));
        //}


        // Updated one with logic Encaped

        [HttpPost]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> ClosePosition(int id, string reasonForClosure, int? linkedCandidateId)
        {
            try
            {
                await _positionService.ClosePositionAsync(id, reasonForClosure, linkedCandidateId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Apply(int positionId)
        {
            ViewBag.PositionId = positionId;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(new Candidate());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Apply(Candidate candidate, int positionId, IFormFile cvFile)
        {
            var extension = System.IO.Path.GetExtension(cvFile.FileName).ToLower();

            // Check if it's a valid file input
            if (cvFile == null || cvFile.Length == 0)
            {
                ModelState.AddModelError("cvFile", "Please upload a valid CV file. Please upload a PDF, DOC, or TXT file.");
                ViewBag.PositionId = positionId;
                return View(candidate);
            }

            // Check for valid file extension 
            if (extension != ".pdf")
            {
                TempData["ErrorMessage"] = "Please upload a valid CV file. Please upload a PDF, DOC, or TXT file.";
                return RedirectToAction("Apply");
            }

            // Check if a candidate with the same email already exists
            var existingCandidate = await _candidateservice.GetCandidateByEmail(candidate.Email);
            if (existingCandidate != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this position.";
                return RedirectToAction("Apply", new { positionId });
            }

            // Save the resume file
            var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Giving the file name as user name then resume 
            var uniqueFileName = $"{candidate.Name}_{cvFile.FileName}";
            var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(fileStream);
            }

            candidate.ResumePath = "/uploads/" + uniqueFileName;

            if (string.IsNullOrEmpty(candidate.ResumePath))
            {
                ModelState.AddModelError("ResumePath", "Resume file is required.");
                ViewBag.PositionId = positionId;
                return View(candidate);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.PositionId = positionId;
                return View(candidate);
            }

            // Set default values
            candidate.CreatedDate = DateTime.Now;
            candidate.ProfileStatus = "Applied";
            candidate.CandidateSkills = new List<CandidateSkill>();
            candidate.CandidateDocuments = new List<CandidateDocument>();
            candidate.CandidateReviews = new List<CandidateReview>();
            candidate.Interviews = new List<Interview>();
            candidate.FinalSelections = new List<FinalSelection>();
            candidate.CandidateStatusHistories = new List<CandidateStatusHistory>();
            candidate.DocumentVerifications = new List<DocumentVerification>();

            await _candidateservice.AddCandidate(candidate);

            TempData["SuccessMessage"] = "Application submitted successfully!";
            return RedirectToAction("Apply", new { positionId });
        }

        private async Task<string> ExtractTextFromCV(IFormFile file)
        {
            string text = "";
            var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    if (extension == ".pdf")
                        text = ExtractTextFromPdf(stream);
                    else if (extension == ".doc" || extension == ".docx")
                        text = ExtractTextFromWord(stream);
                    else if (extension == ".txt")
                        text = await new StreamReader(stream).ReadToEndAsync();
                    else
                        throw new NotSupportedException("Unsupported file format. Please upload a PDF, DOC, or TXT file.");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error processing CV: " + ex.Message;
                return "";
            }

            return text;
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            using (PdfReader reader = new PdfReader(pdfStream))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }

        private string ExtractTextFromWord(Stream docStream)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(docStream, false))
            {
                return doc.MainDocumentPart.Document.Body.InnerText;
            }
        }

        private static string ExtractDetail(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text)) return "";
            var match = Regex.Match(text, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }

    }
}
