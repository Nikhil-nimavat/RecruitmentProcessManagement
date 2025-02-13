using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly Data.ApplicationDbContext _context;
        public CandidateRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        //{
        //    List<Candidate> candidates = await _context.Candidates.ToListAsync();
        //    return candidates;
        //}

        public async Task<Candidate> GetCandidateById(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            return candidate;
        }
        public async Task AddCandidate(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
            await _context.SaveChangesAsync();
        }
        public async Task<Candidate> GetCandidateByEmail(string email)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
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

        public async Task<IEnumerable<Skill>> GetSkillList()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<Skill> GetSkillById(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            return skill;
        }

        public async Task AddSkill(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSkill(Skill skill)
        {
            _context.Skills.Update(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSkill(int id)
        {
            var Skill = _context.Skills.Find(id);
            if (Skill != null)
            {
                _context.Skills.Remove(Skill);
                await _context.SaveChangesAsync();
            }

        }
    }
}
