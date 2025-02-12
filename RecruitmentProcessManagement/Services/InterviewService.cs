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
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public InterviewService(
            ApplicationDbContext context,
              IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<List<IdentityUser>> GetBestInterviewers(int positionId)
        {
            var positionSkills = await _context.PositionSkills
                .Where(ps => ps.PositionID == positionId)
                .Select(ps => ps.SkillID)
                .ToListAsync();

            var bestInterviewers = await _context.Users
                .Where(u => _context.InterviewerSkills
                .Any(interviewerSkill => interviewerSkill.InterviewerID.ToString() == u.Id && positionSkills.Contains(interviewerSkill.SkillID)))
                .ToListAsync();

            return bestInterviewers;
        }
        public async Task<bool> AssignInterviewers(int candidateId, int positionId)
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
                    InterviewerID = Convert.ToInt32(interviewer.Id) 
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SendMeetingInvites(int candidateId, List<string> interviewerIds, DateTime interviewDate)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);

            var stringInterviewerIds = interviewerIds.Select(id => id.ToString()).ToList();

            var interviewers = await _context.Users
                .Where(u => stringInterviewerIds.Contains(u.Id))
                .ToListAsync();

            if (candidate == null || !interviewers.Any())
            {
                throw new Exception("Invalid candidate or interviewers.");
            }

            string subject = "Interview Scheduled - " + interviewDate.ToString("yyyy-MM-dd HH:mm");
            string body = $"Dear {candidate.Name},\n\nYou have an interview scheduled on {interviewDate}. \nPlease be prepared.\n\nBest regards,\nHR Team";

            await _emailService.SendEmailAsync(candidate.Email, subject, body);

            foreach (var interviewer in interviewers)
            {
                string interviewerBody = $"Dear {interviewer.UserName},\n\nYou are assigned to interview {candidate.Name} on {interviewDate}.\n\nBest regards,\nHR Team";
                await _emailService.SendEmailAsync(interviewer.Email, subject, interviewerBody);
            }
        }
    }
}
