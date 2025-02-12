using System.Data;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class BulkHiringController : Controller
    {
        private readonly IBulkHiringService _bulkHiringService;

        public BulkHiringController(IBulkHiringService bulkHiringService)
        {
            _bulkHiringService = bulkHiringService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UploadBulkCandidates()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadBulkCandidates(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a valid Excel file!";
                return RedirectToAction("HRDashboard");
            }

            using (var stream = excelFile.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    await _bulkHiringService.ImportCandidatesFromExcel(dt);
                }
            }

            TempData["SuccessMessage"] = "Bulk candidates uploaded successfully!";
            return RedirectToAction("HRDashboard");
        }
    }
}
