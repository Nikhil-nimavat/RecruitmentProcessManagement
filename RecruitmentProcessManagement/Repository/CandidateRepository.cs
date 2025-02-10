using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        //{
        //    List<Candidate> candidates = await _context.Candidates.ToListAsync();
        //    return candidates;
        //}

        //public async Task<Candidate> GetCandidateById(int id)
        //{
        //    var candidate = await _context.Candidates.FindAsync(id);
        //    return candidate;
        //}
        public async Task AddCandidate(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateCandidate(Candidate candidate)
        //{
        //    _context.Candidates.Update(candidate);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task DeleteCandidateById(int id)
        //{
        //   var candidate = _context.Candidates.Find(id);
        //    if (candidate != null)
        //    { 
        //        _context.Candidates.Remove(candidate);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
