using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class InterviewerRepository : IInterviewerRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Interviewer>> GetAllAsync()
        {
            return await _context.Interviewers.ToListAsync();
        }

        public async Task<Interviewer> GetByIdAsync(int id)
        {
            return await _context.Interviewers.FindAsync(id);
        }

        public async Task AddAsync(Interviewer interviewer)
        {
            _context.Interviewers.Add(interviewer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Interviewer interviewer)
        {
            _context.Interviewers.Update(interviewer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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
