using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CandidateReviewController : Controller
    {
        private readonly ICandidateReviewService _reviewService;
        private readonly ApplicationDbContext _context;

        public CandidateReviewController(ICandidateReviewService reviewService,
            ApplicationDbContext context)
        {
            _reviewService = reviewService;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetCandidateScreeningHistory();

            var reviewViewModels = reviews.Select(r => new CandidateReviewViewModel
            {
                CandidateID = r.CandidateID,
                PositionID = r.PositionID,
                Status = r.Status,
                ReviewDate = r.ReviewDate
            }).ToList();

            return View(reviewViewModels);
        }

        // Error Code
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ReviewCandidate()
        {
            return View();
        }

        // Static code for intial testing only
        //public async Task<IActionResult> ReviewCandidate(int candidateId, int positionId)
        //{
        //    var candidates = await _reviewService.GetCandidatesForReview(positionId);
        //    var candidate = candidates.FirstOrDefault(c => c.CandidateID == candidateId);
        //    if (candidate == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new CandidateReviewViewModel
        //    {
        //        CandidateID = candidate.CandidateID,
        //        PositionID = positionId,
        //        ReviewerID = 1, // Example Reviewer
        //        Comments = "",
        //        Status = "",
        //        CandidateSkills = new List<CandidateSkillViewModel>
        //    {
        //        new CandidateSkillViewModel { SkillID = 1, YearsOfExperience = 0 },
        //        new CandidateSkillViewModel { SkillID = 2, YearsOfExperience = 0 }
        //    }
        //    };

        //    return View(viewModel);
        //}

        //Updated for the view alignment and encp logic

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReviewCandidate(int candidateId, int positionId)
        {
            var lastReview = await _context.CandidateReviews
                .Where(r => r.CandidateID == candidateId)
                .OrderByDescending(r => r.ReviewDate)
                .FirstOrDefaultAsync();

            if (lastReview != null)
            {
                ViewBag.PreviousScreeningDate = lastReview.ReviewDate.ToString("yyyy-MM-dd");
            }

            var skills = await _context.Skills.ToListAsync();

            var viewModel = new CandidateReviewViewModel
            {
                CandidateID = candidateId,
                PositionID = positionId,
                CandidateSkills = skills.Select(s => new CandidateSkillViewModel
                {
                    SkillID = s.SkillID,
                    YearsOfExperience = 0
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SubmitReview(CandidateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ReviewCandidate", model);
            }

            bool success = await _reviewService.SubmitReview(
                model.CandidateID,
                model.PositionID,
                model.ReviewerID,
                model.Comments,
                model.Status,
                model.CandidateSkills.Select(s => new CandidateSkill
                {
                    SkillID = s.SkillID,
                    YearsOfExperience = s.YearsOfExperience
                }).ToList()
            );

            if (!success)
            {
                ModelState.AddModelError("", "Failed to submit review.");
                return View("ReviewCandidate", model);
            }

            return RedirectToAction("Index");
        }
    }
}
