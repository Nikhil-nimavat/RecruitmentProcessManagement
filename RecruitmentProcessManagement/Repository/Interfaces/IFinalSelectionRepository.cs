using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IFinalSelectionRepository
    {
        Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate);
        Task<Candidate> GetCandidateById(int candidateId);
        Task MarkCandidateAsHired(int candidateId, string offerLetterPath);
    }
}
