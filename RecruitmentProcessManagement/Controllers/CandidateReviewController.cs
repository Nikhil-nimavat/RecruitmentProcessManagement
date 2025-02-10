﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetCandidateScreeningHistory("1"); // Example CandidateID
            return View(reviews);
        }

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

        //Updated for the view alignment
        public async Task<IActionResult> ReviewCandidate(string candidateId, int positionId)
        {
            var lastReview = await _context.CandidateReviews
                .Where(r => r.CandidateID == candidateId)
                .OrderByDescending(r => r.ReviewDate)
                .FirstOrDefaultAsync();

            if (lastReview != null)
            {
                ViewBag.PreviousScreeningDate = lastReview.ReviewDate.ToString("yyyy-MM-dd");
            }

            return View(new CandidateReviewViewModel { CandidateID = candidateId, PositionID = positionId });
        }

        [HttpPost]
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
