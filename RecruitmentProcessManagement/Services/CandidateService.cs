using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ApplicationDbContext _context;

        public CandidateService(ICandidateRepository candidateRepository, ApplicationDbContext context)
        {
            _candidateRepository = candidateRepository;
            _context = context; 
        }

        public async Task AddCandidate(Candidate candidate)
        {
            await _candidateRepository.AddCandidate(candidate);
        }

        public async Task<Candidate> GetCandidateById(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            return candidate;
        }
    }
}