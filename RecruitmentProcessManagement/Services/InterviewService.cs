using System;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewService _interviewService;
        private readonly ApplicationDbContext _context;
        public InterviewService(IInterviewService interviewService
            ,ApplicationDbContext context)
        {
            _interviewService = interviewService;
            _context = context;
        }

        // error code

        //public async Task<List<Users>> GetBestInterviewers(int positionId)
        //{
        //    var positionSkills = await _context.PositionSkills
        //        .Where(ps => ps.PositionID == positionId)
        //        .Select(ps => ps.SkillID)
        //        .ToListAsync();

        //    // Syntax error as "is." gives nothing
        //    var bestInterviewers = await _context.Users
        //        .Where(u => _context.InterviewerSkills
        //        .Any(is => is.InterviewerID == u.Id && positionSkills.Contains(is.SkillID)))
        //        .ToListAsync();

        //    return bestInterviewers;
        //}
        public async Task<bool> AssignInterviewers(string candidateId, int positionId)
        {
            var interview = await _context.Interviews
                .FirstOrDefaultAsync(i => i.CandidateID == candidateId && i.PositionID == positionId);

            if (interview == null) return false;

            // _context.user changed to role to find interviewer role
            var interviewers = _context.Roles.Where(u => u.Name == "Interviewer").Take(2).ToList();


            // Refined the Interviewer login and check if interview needs interviewer_Id 
            foreach (var interviewer in interviewers)
            {
                _context.Interviews.Add(new Interview
                {
                    InterviewID = interview.InterviewID,
                    InterviewerID = interviewer.Id
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SendMeetingInvites(string candidateId, List<string> interviewerIds, DateTime interviewDate)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            var interviewers = await _context.Users
                .Where(u => interviewerIds.Contains(u.Id))
                .ToListAsync();

            string subject = "Interview Scheduled - " + interviewDate.ToString("yyyy-MM-dd HH:mm");
            string body = $"Dear {candidate.Name},\n\nYou have an interview scheduled on {interviewDate}. \nPlease be prepared.\n\nBest regards,\nHR Team";


            // After configuring email service
            // Send invite to candidate

            // await _emailService.SendEmailAsync(candidate.Email, subject, body);

            //// Send invite to interviewers
            //foreach (var interviewer in interviewers)
            //{
            //    await _emailService.SendEmailAsync(interviewer.Email, subject, "You are assigned to interview " + candidate.Name);
            //}
        }
    }
}
