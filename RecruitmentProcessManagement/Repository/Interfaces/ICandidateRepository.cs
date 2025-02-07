using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface ICandidateRepository
    {
        Task<IEnumerable<Candidate>> GetAllCandidatesAsync();

        Task<Candidate> GetCandidateById(int id);

        Task AddCandidate(Candidate candidate);

        Task UpdateCandidate(Candidate candidate);

        Task DeleteCandidateById(int id);
    }
}
