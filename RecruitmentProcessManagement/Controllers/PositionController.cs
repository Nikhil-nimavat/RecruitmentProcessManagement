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
        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Candidates = await _context.Candidates.ToListAsync();
            return View(positions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        //[HttpGet]
        //public IActionResult Apply(int positionId)
        //{
        //    ViewBag.PositionId = positionId;
        //    return View();
        //}

        ////[HttpPost]
        ////public async Task<IActionResult> Apply(Candidate candidate, IFormFile cvFile)
        ////{
        ////    if (ModelState.IsValid && cvFile != null)
        ////    {
        ////        // Save CV (Resume) File
        ////        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        ////        if (!Directory.Exists(uploadsFolder))
        ////        {
        ////            Directory.CreateDirectory(uploadsFolder);
        ////        }

        ////        var uniqueFileName = $"{Guid.NewGuid()}_{cvFile.FileName}";
        ////        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        ////        using (var fileStream = new FileStream(filePath, FileMode.Create))
        ////        {
        ////            await cvFile.CopyToAsync(fileStream);
        ////        }

        ////        candidate.ResumePath = "/uploads/" + uniqueFileName;

        ////        await _candidateService.AddCandidate(candidate);
        ////        return RedirectToAction("Index", "Position");
        ////    }

        ////    return View(candidate);
        ////}

        //[HttpPost]
        //public async Task<IActionResult> Apply(Candidate candidate, IFormFile cvFile)
        //{
        //    if (cvFile != null)
        //    {
        //        string extractedText = await ExtractTextFromCV(cvFile);

        //        candidate.ExtractedText = extractedText;

        //        candidate.Name = ExtractDetail(extractedText, @"(?i)Name[:\s]+([^\n\r]+)");
        //        candidate.Email = ExtractDetail(extractedText, @"(?i)[\w\.-]+@[\w\.-]+\.\w+");
        //        candidate.PhoneNumber = ExtractDetail(extractedText, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}");

        //         TempData["ExtractedCandidate"] = JsonConvert.SerializeObject(candidate);
        //    }

        //    return RedirectToAction("ApplyWithData", new { positionId = ViewBag.PositionId });
        //}

        ////[HttpPost]
        ////public async Task<IActionResult> Apply(Candidate candidate, IFormFile cvFile)
        ////{
        ////    if (cvFile != null)
        ////    {
        ////        string extractedText = await ExtractTextFromCV(cvFile);

        ////        // Store extracted text in the candidate model
        ////        candidate.ExtractedText = extractedText;

        ////        // Extract details using regex
        ////        candidate.FullName = ExtractDetail(extractedText, @"(?i)Name[:\s]+([^\n\r]+)");
        ////        candidate.Email = ExtractDetail(extractedText, @"(?i)[\w\.-]+@[\w\.-]+\.\w+");
        ////        candidate.PhoneNumber = ExtractDetail(extractedText, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}");

        ////        // Store candidate data in TempData for the next step
        ////        TempData["ExtractedCandidate"] = JsonConvert.SerializeObject(candidate);
        ////    }

        ////    return RedirectToAction("ApplyWithData", new { positionId = ViewBag.PositionId });
        ////}

        //[HttpGet]
        //public IActionResult ApplyWithData(int positionId)
        //{
        //    ViewBag.PositionId = positionId;

        //    var extractedCandidateJson = TempData["ExtractedCandidate"] as string;
        //    var candidate = !string.IsNullOrEmpty(extractedCandidateJson)
        //        ? JsonConvert.DeserializeObject<Candidate>(extractedCandidateJson)
        //        : new Candidate { Name = "", Email = "", PhoneNumber = "" }; 

        //    return View("Apply", candidate);
        //}

        //[HttpGet]
        //public IActionResult Apply(int positionId)
        //{
        //    ViewBag.PositionId = positionId;
        //    return View(new Candidate { Name = "", Email = "", PhoneNumber = ""});
        //}

        //[HttpPost]
        //public async Task<IActionResult> Apply(Candidate candidate, IFormFile cvFile, int positionId)
        //{
        //    if (cvFile == null || cvFile.Length == 0)
        //    {
        //        TempData["ErrorMessage"] = "Please upload a valid CV file.";
        //        return RedirectToAction("Apply", new { positionId });
        //    }

        //    var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = $"{Guid.NewGuid()}_{cvFile.FileName}";
        //    var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await cvFile.CopyToAsync(fileStream);
        //    }

        //    candidate.ResumePath = "/uploads/" + uniqueFileName;

        //    string extractedText = await ExtractTextFromCV(cvFile);

        //    candidate.ExtractedText = extractedText;
        //    candidate.Name = ExtractDetail(extractedText, @"(?i)Name[:\s]+([^\n\r]+)");
        //    candidate.Email = ExtractDetail(extractedText, @"(?i)[\w\.-]+@[\w\.-]+\.\w+");
        //    candidate.PhoneNumber = ExtractDetail(extractedText, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}");

        //    TempData["ExtractedCandidate"] = JsonConvert.SerializeObject(candidate);

        //    return RedirectToAction("ApplyWithData", new { positionId });
        //}

        //[HttpGet]
        //public IActionResult ApplyWithData(int positionId)
        //{
        //    ViewBag.PositionId = positionId;

        //    var extractedCandidateJson = TempData["ExtractedCandidate"] as string;
        //    var candidate = !string.IsNullOrEmpty(extractedCandidateJson)
        //        ? JsonConvert.DeserializeObject<Candidate>(extractedCandidateJson)
        //        : new Candidate { Name = "", Email = "", PhoneNumber = ""};

        //    return View("Apply", candidate);
        //}

        //[HttpGet]
        //public IActionResult Apply(int positionId)
        //{
        //    ViewBag.PositionId = positionId;

        //    if (ViewData["ExtractedCandidate"] is Candidate extractedCandidate)
        //    {
        //        return View(extractedCandidate);
        //    }

        //    return View(new Candidate { Name = "", PhoneNumber = "", Email = ""});
        //}

        //[HttpPost]
        //public async Task<IActionResult> UploadCV(int positionId, IFormFile cvFile)
        //{
        //    if (cvFile == null || cvFile.Length == 0)
        //    {
        //        TempData["ErrorMessage"] = "Please upload a valid CV file.";
        //        return RedirectToAction("Apply", new { positionId });
        //    }

        //    var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = $"{Guid.NewGuid()}_{cvFile.FileName}";
        //    var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await cvFile.CopyToAsync(fileStream);
        //    }

        //    string extractedText = await ExtractTextFromCV(cvFile);

        //    var candidate = new Candidate
        //    {
        //        ResumePath = "/uploads/" + uniqueFileName,
        //        ExtractedText = extractedText,
        //        Name = ExtractDetail(extractedText, @"(?i)Name[:\s]+([^\n\r]+)"),
        //        Email = ExtractDetail(extractedText, @"(?i)[\w\.-]+@[\w\.-]+\.\w+"),
        //        PhoneNumber = ExtractDetail(extractedText, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}")
        //    };

        //    ViewData["ExtractedCandidate"] = candidate;

        //    return View("Apply", candidate); 
        //}

        //[HttpPost]
        //public async Task<IActionResult> Apply(Candidate candidate, int positionId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _candidateservice.AddCandidate(candidate);
        //        TempData["SuccessMessage"] = "Application submitted successfully!";
        //        return RedirectToAction("Apply", new { positionId });
        //    }

        //    return View(candidate); 
        //}

        [HttpGet]
        public IActionResult Apply(int positionId)
        {
            ViewBag.PositionId = positionId;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            if (ViewData["ExtractedCandidate"] is Candidate extractedCandidate)
            {
                return View(extractedCandidate);
            }

            return View(new Candidate { Name = "", Email = "", PhoneNumber = ""});
        }

        [HttpPost]
        public async Task<IActionResult> UploadCV(int positionId, IFormFile cvFile)
        {
            if (cvFile == null || cvFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a valid CV file.";
                return RedirectToAction("Apply", new { positionId });
            }

            var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{cvFile.FileName}";
            var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(fileStream);
            }

            string extractedText = await ExtractTextFromCV(cvFile);

            var candidate = new Candidate
            {
                ResumePath = "/uploads/" + uniqueFileName,
                ExtractedText = extractedText,
                Name = ExtractDetail(extractedText, @"(?i)Name[:\s]+([^\n\r]+)"),
                Email = ExtractDetail(extractedText, @"(?i)[\w\.-]+@[\w\.-]+\.\w+"),
                PhoneNumber = ExtractDetail(extractedText, @"\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}")
            };

            ViewData["ExtractedCandidate"] = candidate;

            return View("Apply", candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(Candidate candidate, int positionId)
        {
            if (!ModelState.IsValid)
            {
                return View(candidate);
            }

            var existingCandidate = await _candidateservice.GetCandidateById(candidate.CandidateID);
            if (existingCandidate != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this position.";
                return RedirectToAction("Apply", new { positionId });
            }

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
