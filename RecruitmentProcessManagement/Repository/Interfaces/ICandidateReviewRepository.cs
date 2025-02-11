using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface ICandidateReviewRepository
    {
        Task<bool> AssignReviewer(int positionId, int reviewerId);
        Task<List<Candidate>> GetCandidatesForReview(int positionId);
        Task<CandidateReview> GetReview(int candidateId, int positionId, int reviewerId);
        Task<bool> SubmitReview(CandidateReview review, Candidate candidate, List<CandidateSkill> skills);
        Task<List<CandidateReview>> GetCandidateScreeningHistory(int candidateId);
    }
}
