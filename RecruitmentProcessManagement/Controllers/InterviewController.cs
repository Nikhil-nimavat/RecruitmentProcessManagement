﻿using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Models.ViewModels;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    [Authorize(Roles = "Admin, Recruiter, Reviewer, HR, Interviewer")]
    public class InterviewController : Controller
    {
        private readonly IInterviewService _interviewService;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext _context;

        public InterviewController(INotificationService notificationService, IInterviewService interviewService, ApplicationDbContext context)
        {
            _notificationService = notificationService;
            _interviewService = interviewService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBestInterviewers(int positionId)
        {
            var interviewers = await _interviewService.GetBestInterviewers(positionId);
            return Json(interviewers);
        }

        [HttpGet]
        public async Task<IActionResult> ManageInterviews()
        {
            var interviews = await _context.InterviewRounds
                .Include(ir => ir.Interview)
                    .ThenInclude(i => i.Candidate)
                .Include(ir => ir.Interview)
                    .ThenInclude(i => i.Position)
                .Select(ir => new ManageInterviewsViewModel
                {
                    InterviewId = ir.Interview.InterviewID,
                    CandidateName = ir.Interview.Candidate.Name,
                    PositionTitle = ir.Interview.Position.JobTitle,
                    InterviewDate = ir.Interview.InterviewDate,
                    InterviewType = ir.RoundType,
                    Status = ir.Interview.Status,
                    RoundNumber = ir.RoundNumber
                })
                .OrderBy(ir => ir.CandidateName)
                .ThenBy(ir => ir.RoundNumber)
                .ToListAsync();

            return View(interviews);
        }

        [HttpGet]

        public async Task<IActionResult> InterviewDetails(int interviewId, int roundNumber)
        {
            var interviewRound = await _context.InterviewRounds
                .Include(ir => ir.Interview)
                    .ThenInclude(i => i.Candidate)
                .Include(ir => ir.Interview)
                    .ThenInclude(i => i.Position)
                .Include(ir => ir.InterviewRoundInterviewers)
                    .ThenInclude(iri => iri.Interviewer)
                .FirstOrDefaultAsync(ir => ir.InterviewID == interviewId && ir.RoundNumber == roundNumber);

            if (interviewRound == null)
            {
                TempData["ErrorMessage"] = "Interview round not found.";
                return RedirectToAction("ManageInterviews");
            }

            var interviewers = interviewRound.InterviewRoundInterviewers
                .Select(iri => iri.Interviewer.FullName)
                .Distinct()
                .ToList();

            var model = new InterviewDetailsViewModel
            {
                InterviewId = interviewRound.InterviewID,
                CandidateName = interviewRound.Interview.Candidate.Name,
                PositionTitle = interviewRound.Interview.Position.JobTitle,
                InterviewType = interviewRound.RoundType,
                InterviewDate = interviewRound.Interview.InterviewDate,
                Status = interviewRound.Interview.Status,
                Interviewers = interviewers,
                Feedback = interviewRound.Feedback ?? "Not Provided",
                Rating = interviewRound.Rating,
                RoundNumber = roundNumber
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleInterview(int? interviewId)
        {
            var model = new Models.ViewModels.ScheduleInterviewViewModel
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

                Interviewers = await _context.Interviewers
                    .Select(i => new SelectListItem
                    {
                        Value = i.InterviewerID.ToString(),
                        Text = i.FullName
                    }).ToListAsync()
            };

            if (interviewId != null)
            {
                var interview = await _context.Interviews
                    .Include(i => i.InterviewRounds)
                    .FirstOrDefaultAsync(i => i.InterviewID == interviewId);

                if (interview != null)
                {
                    model.CandidateId = interview.CandidateID;
                    model.PositionId = interview.PositionID;
                    model.InterviewDate = interview.InterviewDate;
                    model.InterviewType = interview.InterviewType;
                    model.InterviewerIds = await _context.InterviewRoundInterviewers
                        .Where(iri => iri.InterviewRound.InterviewID == interview.InterviewID)
                        .Select(iri => iri.InterviewerID.ToString())
                        .ToListAsync();
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ScheduleInterview(Models.ViewModels.ScheduleInterviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Candidates = await _context.Candidates
                    .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = c.CandidateID.ToString(),
                        Text = c.Name
                    }).ToListAsync();

                model.Positions = await _context.Positions
                    .Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = p.PositionID.ToString(),
                        Text = p.JobTitle
                    }).ToListAsync();

                model.Interviewers = await _context.Interviewers
                    .Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Value = i.InterviewerID.ToString(),
                        Text = i.FullName
                    }).ToListAsync();

                return View(model);
            }

            var existingInterview = await _context.Interviews
                .FirstOrDefaultAsync(i => i.CandidateID == model.CandidateId && i.PositionID == model.PositionId);

            Interview interview;

            if (existingInterview == null)
            {
                interview = new Interview
                {
                    CandidateID = model.CandidateId,
                    PositionID = model.PositionId,
                    InterviewType = model.InterviewType,
                    InterviewDate = model.InterviewDate,
                    Status = "Scheduled"
                };

                _context.Interviews.Add(interview);
                await _context.SaveChangesAsync();
            }
            else
            {
                interview = existingInterview;
            }

            int nextRoundNumber = (await _context.InterviewRounds
            .Where(ir => ir.InterviewID == interview.InterviewID)
            .Select(ir => ir.RoundNumber)
            .ToListAsync())
            .DefaultIfEmpty(0)
            .Max() + 1;


            var interviewRound = new InterviewRound
            {
                InterviewID = interview.InterviewID,
                RoundNumber = nextRoundNumber,
                RoundType = model.InterviewType,
                Feedback = "",
                Rating = 0,
                CreatedDate = DateTime.Now
            };

            _context.InterviewRounds.Add(interviewRound);
            await _context.SaveChangesAsync();

            await _interviewService.SendMeetingInvites(model.CandidateId, model.InterviewerIds, model.InterviewDate);

            TempData["SuccessMessage"] = "Interview scheduled successfully. Emails sent!";
            return RedirectToAction("ManageInterviews");
        }

        [HttpPost]
        public async Task<IActionResult> DefineInterviewRounds(int positionId, List<string> roundTypes)
        {
            var existingRounds = await _context.InterviewRounds
                .Where(r => r.Interview.PositionID == positionId)
                .ToListAsync();

            if (existingRounds.Any())
            {
                _context.InterviewRounds.RemoveRange(existingRounds);
            }

            foreach (var roundType in roundTypes)
            {
                _context.InterviewRounds.Add(new InterviewRound
                {
                    InterviewID = positionId,
                    RoundType = roundType,
                    RoundNumber = roundTypes.IndexOf(roundType) + 1
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageInterviewRounds", new { positionId });
        }

        [HttpGet]
        public async Task<IActionResult> GetScheduledInterviews()
        {
            var interviews = await _context.Interviews
                .Include(i => i.Candidate)
                .Include(i => i.Position)
                .Select(i => new {
                    title = $"{i.Candidate.Name} - {i.Position.JobTitle}",
                    start = i.InterviewDate,
                    url = Url.Action("InterviewDetails", new { interviewId = i.InterviewID })
                })
                .ToListAsync();

            return Json(interviews);
        }

        [HttpPost]
        public async Task<IActionResult> RescheduleInterview(int interviewId, DateTime newDate)
        {
            var interview = await _context.Interviews.FindAsync(interviewId);
            if (interview == null) return NotFound();

            interview.InterviewDate = newDate;
            interview.Status = "Rescheduled";
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageInterviews");
        }

        [HttpPost]
        public async Task<IActionResult> CancelInterview(int interviewId)
        {
            try
            {
                var interview = await _context.Interviews.FindAsync(interviewId);
                if (interview == null)
                {
                    TempData["ErrorMessage"] = "Interview not found.";
                    return RedirectToAction("ManageInterviews");
                }

                _context.Interviews.Remove(interview);
                await _context.SaveChangesAsync();

                var candidate = await _context.Candidates
                    .Where(c => c.CandidateID == interview.CandidateID)
                    .Select(c => c.CandidateID)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(candidate.ToString()))
                {
                    await _notificationService.SendInterviewNotification(candidate, "Your interview has been canceled.");
                }

                TempData["SuccessMessage"] = "Interview canceled successfully.";
            }
            catch (Exception)
            {
                TempData["SuccessMessage"] = "Interview canceled successfully.";
            }

            return RedirectToAction("ManageInterviews");
        }

        [HttpPost]
        public async Task<IActionResult> BulkScheduleInterviews(List<int> candidateIds, int positionId, DateTime interviewDate)
        {
            foreach (var candidateId in candidateIds)
            {
                var interview = new Interview
                {
                    CandidateID = candidateId,
                    PositionID = positionId,
                    InterviewDate = interviewDate,
                    Status = "Scheduled"
                };

                _context.Interviews.Add(interview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ManageInterviews", new { positionId });
        }

        [HttpGet]
        [Authorize(Roles = "Interviewer, Admin, HR")]
        public async Task<IActionResult> GiveFeedback(int id, int roundnumber)
        {
            var interviewRound = await _context.InterviewRounds
                .FirstOrDefaultAsync(r => r.InterviewID == id);
            var roundnumbers = await _context.InterviewRounds
                 .FirstOrDefaultAsync(r => r.RoundNumber == roundnumber);

            if (interviewRound == null)
            {
                return NotFound();
            }

            var model = new InterviewFeedbackViewModel
            {
                InterviewRoundID = interviewRound.InterviewRoundID,
                InterviewID = interviewRound.InterviewID,
                RoundNumber = roundnumbers.RoundNumber
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Interviewer, Admin, HR")]
        public async Task<IActionResult> GiveFeedback(InterviewFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var interviewRound = await _context.InterviewRounds
                .FirstOrDefaultAsync(r => r.InterviewID == model.InterviewID);

            var roundnumber = await _context.InterviewRounds
                .FirstOrDefaultAsync(r => r.RoundNumber == model.RoundNumber);

            if (interviewRound == null)
            {
                return NotFound();
            }

            //interviewRound.Feedback = model.Feedback;
            //interviewRound.Rating = model.Rating;
            roundnumber.Feedback = model.Feedback;
            roundnumber.Rating = model.Rating;

            _context.Update(roundnumber);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Feedback submitted successfully!";
            return RedirectToAction("InterviewDetails");
        }
    }
}
