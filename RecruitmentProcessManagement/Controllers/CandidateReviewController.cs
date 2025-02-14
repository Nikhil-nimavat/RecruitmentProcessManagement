using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Models.ViewModels;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CandidateReviewController : Controller
    {
        private readonly ICandidateReviewService _reviewService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CandidateReviewController(ICandidateReviewService reviewService,
            ApplicationDbContext context, UserManager<IdentityUser> userManager )
        {
            _reviewService = reviewService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Reviewer")]
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

        [HttpGet]
        [Authorize(Roles = "Admin, Reviewer")]
        public async Task<IActionResult> ReviewCandidate()
        {
            var model = new ReviewCandidateViewModel
            {
                Candidates = await _context.Candidates
                    .Select(c => new SelectListItem
                    {
                        Value = c.CandidateID.ToString(),
                        Text = c.Name
                    }).ToListAsync(),

                Positions = await _context.Positions
                    .Select(p => new SelectListItem
                    {
                        Value = p.PositionID.ToString(),
                        Text = p.JobTitle
                    }).ToListAsync(),

                SkillsList = await _context.Skills
                    .Select(s => new SelectListItem
                    {
                        Value = s.SkillID.ToString(),
                        Text = s.SkillName
                    }).ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> LoadCandidateSkills(int candidateId)
        {
            var candidateSkills = await _context.CandidateSkills
                .Where(cs => cs.CandidateID == candidateId)
                .Include(cs => cs.Skill)
                .Select(cs => new CandidateSkillViewModel
                {
                    SkillID = cs.SkillID,
                    YearsOfExperience = cs.YearsOfExperience
                }).ToListAsync();

            return Json(candidateSkills);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Reviewer")]
        public async Task<IActionResult> SubmitReview(ReviewCandidateViewModel model)
        {
            var reviewerId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(reviewerId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.Candidates = await _context.Candidates
                    .Select(c => new SelectListItem
                    {
                        Value = c.CandidateID.ToString(),
                        Text = c.Name
                    }).ToListAsync();

                model.Positions = await _context.Positions
                    .Select(p => new SelectListItem
                    {
                        Value = p.PositionID.ToString(),
                        Text = p.JobTitle
                    }).ToListAsync();

                model.SkillsList = await _context.Skills
                    .Select(s => new SelectListItem
                    {
                        Value = s.SkillID.ToString(),
                        Text = s.SkillName
                    }).ToListAsync();

                return View(model);
            }

            var review = new CandidateReview
            {
                CandidateID = model.CandidateId,
                PositionID = model.PositionId,
                ReviewerID = reviewerId,
                ReviewDate = DateTime.Now,
                Comments = model.Comments,
                Status = model.CurrentStatus 
            };

            _context.CandidateReviews.Add(review);
            await _context.SaveChangesAsync();

            foreach (var skill in model.SelectedSkills)
            {
                var candidateSkill = new CandidateSkill
                {
                    CandidateID = model.CandidateId,
                    SkillID = skill.SkillID,
                    YearsOfExperience = skill.YearsOfExperience
                };

                _context.CandidateSkills.Add(candidateSkill);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Review submitted successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Reviewer")]
        public async Task<IActionResult> StartReview(CandidateReviewSelectionViewModel model)
        {
            if (model.SelectedCandidateID == 0 || model.SelectedPositionID == 0)
            {
                ModelState.AddModelError("", "Please select a candidate and a position.");
                return View("ReviewCandidate", model);
            }

            return RedirectToAction("SubmitReview", new
            {
                candidateId = model.SelectedCandidateID,
                positionId = model.SelectedPositionID
            });
        }

    }
}
