using DocumentFormat.OpenXml.Office2010.Excel;
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
    public class CandidateReviewController : Controller
    {
        private readonly ICandidateReviewService _reviewService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CandidateReviewController(ICandidateReviewService reviewService,
            ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _reviewService = reviewService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Reviewer, HR")]
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetCandidateScreeningHistory();

            var reviewViewModels = reviews.Select(r => new CandidateReviewViewModel
            {
                CandidateID = r.CandidateID,
                CandidateName = r.Candidate.Name,

                PositionID = r.PositionID,
                JobTitle = r.Position.JobTitle,

                Status = r.Status,
                ReviewDate = r.ReviewDate
            }).ToList();

            return View(reviewViewModels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Reviewer, HR")]
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
        [Authorize(Roles = "Admin, Reviewer, HR")]
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

            var existingReview = await _context.CandidateReviews
                .FirstOrDefaultAsync(r => r.CandidateID == model.CandidateId && r.PositionID == model.PositionId);

            if (existingReview != null)
            {
                TempData["ErrorMessage"] = "A review for this candidate and position already exists.";
                return RedirectToAction("ReviewCandidate");
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
        [Authorize(Roles = "Admin, Reviewer, HR")]
        public IActionResult StartReview(CandidateReviewSelectionViewModel model)
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

        [HttpGet]
        [Authorize(Roles = "Admin, Reviewer, HR")]
        public async Task<IActionResult> EditReview(int candidateId, int positionId)
        {
            var review = await _context.CandidateReviews
                .Include(r => r.Candidate)
                .Include(r => r.Position)
                .FirstOrDefaultAsync(r => r.CandidateID == candidateId && r.PositionID == positionId);

            if (review == null)
            {
                return NotFound();
            }

            var model = new ReviewCandidateViewModel
            {
                ReviewID = review.ReviewID,
                CandidateId = review.CandidateID,
                PositionId = review.PositionID,
                CurrentStatus = review.Status,
                Comments = review.Comments,

                Candidates = await _context.Candidates
                    .Select(c => new SelectListItem
                    {
                        Value = c.CandidateID.ToString(),
                        Text = c.Name,
                        Selected = c.CandidateID == review.CandidateID
                    }).ToListAsync(),

                Positions = await _context.Positions
                    .Select(p => new SelectListItem
                    {
                        Value = p.PositionID.ToString(),
                        Text = p.JobTitle,
                        Selected = p.PositionID == review.PositionID
                    }).ToListAsync(),

                SkillsList = await _context.Skills
                    .Select(s => new SelectListItem
                    {
                        Value = s.SkillID.ToString(),
                        Text = s.SkillName
                    }).ToListAsync(),

                SelectedSkills = await _context.CandidateSkills
                    .Where(cs => cs.CandidateID == review.CandidateID)
                    .Select(cs => new CandidateSkillViewModel
                    {
                        SkillID = cs.SkillID,
                        YearsOfExperience = cs.YearsOfExperience
                    }).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Reviewer, HR")]
        public async Task<IActionResult> EditReview(ReviewCandidateViewModel model)
        {
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

            var review = await _context.CandidateReviews
                .FirstOrDefaultAsync(r => r.CandidateID == model.CandidateId && r.PositionID == model.PositionId);

            if (review == null)
            {
                return NotFound();
            }

            review.Comments = model.Comments;
            review.Status = model.CurrentStatus;
            review.ReviewDate = DateTime.Now;

            _context.CandidateReviews.Update(review);
            await _context.SaveChangesAsync();

            var existingSkills = _context.CandidateSkills.Where(cs => cs.CandidateID == model.CandidateId);
            _context.CandidateSkills.RemoveRange(existingSkills);

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

            TempData["SuccessMessage"] = "Review updated successfully!";
            return RedirectToAction("Index");
        }

    }
}
