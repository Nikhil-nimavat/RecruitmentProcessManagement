using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class InterviewerController : Controller
    {
        private readonly IInterviewerService _interviewerService;

        public InterviewerController(IInterviewerService interviewerService)
        {
            _interviewerService = interviewerService;
        }

        public async Task<IActionResult> Index()
        {
            var interviewers = await _interviewerService.GetAllInterviewersAsync();
            return View(interviewers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Interviewer interviewer)
        {
            if (ModelState.IsValid)
            {
                await _interviewerService.AddInterviewerAsync(interviewer);
                return RedirectToAction(nameof(Index));
            }
            return View(interviewer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var interviewer = await _interviewerService.GetInterviewerByIdAsync(id);
            if (interviewer == null)
                return NotFound();

            return View(interviewer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Interviewer interviewer)
        {
            if (ModelState.IsValid)
            {
                await _interviewerService.UpdateInterviewerAsync(interviewer);
                return RedirectToAction(nameof(Index));
            }
            return View(interviewer);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var interviewer = await _interviewerService.GetInterviewerByIdAsync(id);
            if (interviewer == null)
                return NotFound();

            return View(interviewer);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Interviewer interviewer)
        {
            await _interviewerService.DeleteInterviewerAsync(interviewer.InterviewerID);
            return RedirectToAction(nameof(Index));
        }
    }
}
