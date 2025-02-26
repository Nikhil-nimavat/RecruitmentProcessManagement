using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin, HR")]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> PositionWise()
    {
        var reportData = await _reportService.GetPositionWiseReport();
        return View(reportData);
    }

    public async Task<IActionResult> CollegeWise()
    {
        var reportData = await _reportService.GetCollegeWiseReport();
        return View(reportData);
    }

    public async Task<IActionResult> InterviewWise()
    {
        var reportData = await _reportService.GetInterviewWiseReport();
        return View(reportData);
    }
}
