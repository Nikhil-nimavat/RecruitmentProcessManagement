using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateService
    {
        Task<IEnumerable<Candidate>> GetAllCandidatesAsync();

        Task AddCandidate(Candidate candidate);

        Task<Candidate> GetCandidateById(int id);

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
