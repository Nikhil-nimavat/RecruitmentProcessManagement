using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface ICandidateDocumentRepository
    {
        Task AddDocument(CandidateDocument document);

        Task<Candidate> GetCandidateById(int id);
    }
}
