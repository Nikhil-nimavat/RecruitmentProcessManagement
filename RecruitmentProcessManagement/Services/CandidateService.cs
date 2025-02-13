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

        public CandidateService(ICandidateRepository candidateRepository, ApplicationDbContext context)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        {
            return await _candidateRepository.GetAllCandidatesAsync();
        }
        public async Task<Candidate> GetCandidateById(int id)
        {
            return await _candidateRepository.GetCandidateById(id);
        }
        public async Task<Candidate> GetCandidateByEmail(string email)
        {
            return await _candidateRepository.GetCandidateByEmail(email);
        }
        public async Task AddCandidate(Candidate candidate)
        {
            await _candidateRepository.AddCandidate(candidate);
        }
        public async Task UpdateCandidate(Candidate candidate)
        {
            await _candidateRepository.UpdateCandidate(candidate);
        }
        public async Task DeleteCandidateById(int id)
        { 
            await _candidateRepository.DeleteCandidateById(id);
        }
        public async Task<IEnumerable<Skill>> GetSkillList()
        {
            return await _candidateRepository.GetSkillList();
        }
        public async Task<Skill> GetSkillById(int id)
        {
            return await _candidateRepository.GetSkillById(id);   
        }
        public async Task AddSkill(Skill skill)
        {
            await _candidateRepository.AddSkill(skill);
        }
        public async Task UpdateSkill(Skill skill)
        {
            await _candidateRepository.UpdateSkill(skill);
        }
        public async Task DeleteSkill(int id)
        {
            await _candidateRepository.DeleteSkill(id);
        }
    }
}