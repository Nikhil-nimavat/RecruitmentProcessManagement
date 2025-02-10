using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface ICandidateReviewRepository
    {
        Task<bool> AssignReviewer(int positionId, string reviewerId);
        Task<List<Candidate>> GetCandidatesForReview(int positionId);
        Task<CandidateReview> GetReview(string candidateId, int positionId, string reviewerId);
        Task<bool> SubmitReview(CandidateReview review, Candidate candidate, List<CandidateSkill> skills);
        Task<List<CandidateReview>> GetCandidateScreeningHistory(string candidateId);
    }
}
