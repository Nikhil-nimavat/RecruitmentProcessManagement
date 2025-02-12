using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class InterviewerService : IInterviewerService
    {
        private readonly ApplicationDbContext _context;

        public InterviewerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Interviewer>> GetAllInterviewersAsync()
        {
            return await _context.Interviewers.ToListAsync();
        }

        public async Task<Interviewer> GetInterviewerByIdAsync(int id)
        {
            return await _context.Interviewers.FindAsync(id);
        }

        public async Task AddInterviewerAsync(Interviewer interviewer)
        {
            _context.Interviewers.Add(interviewer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInterviewerAsync(Interviewer interviewer)
        {
            _context.Interviewers.Update(interviewer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInterviewerAsync(int id)
        {
            var interviewer = await _context.Interviewers.FindAsync(id);
            if (interviewer != null)
            {
                _context.Interviewers.Remove(interviewer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
