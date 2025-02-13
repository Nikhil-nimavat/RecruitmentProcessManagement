using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateService
    {
        Task AddCandidate(Candidate candidate);

        Task GetCandidateById(int id);
        Task<Candidate> GetCandidateByEmail(string email);

        Task<IEnumerable<Skill>> GetSkillList();

        Task<Skill> GetSkillById(int id);
        Task AddSkill(Skill skill);

        Task UpdateSkill(Skill skill);

        Task DeleteSkill(int id);
    }
}
