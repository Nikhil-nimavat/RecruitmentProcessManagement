using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface ICandidateRepository
    {
        Task<IEnumerable<Candidate>> GetAllCandidatesAsync();

        Task<Candidate> GetCandidateById(int id);

        Task AddCandidate(Candidate candidate);

        Task<Candidate> GetCandidateByEmail(string email);

        Task UpdateCandidate(Candidate candidate);

        Task DeleteCandidateById(int id);

        Task<IEnumerable<Skill>> GetSkillList();

        Task<Skill> GetSkillById(int id);   
        Task AddSkill(Skill skill);

        Task UpdateSkill(Skill skill);

        Task DeleteSkill(int id);

    }
}
