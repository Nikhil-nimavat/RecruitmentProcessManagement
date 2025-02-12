using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IFinalSelectionService
    {
        Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate);
        Task<Candidate> GetCandidateById(int candidateId);
        Task MarkCandidateAsHired(int candidateId, string offerLetterPath);
    }
}
