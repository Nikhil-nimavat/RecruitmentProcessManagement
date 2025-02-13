using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateReviewService
    {
        Task<bool> AssignReviewer(int positionId, int reviewerId);
        Task<List<Candidate>> GetCandidatesForReview(int positionId);
        Task<bool> SubmitReview(int candidateId, int positionId,
            int reviewerId, string comments,
            string status, List<CandidateSkill> candidateSkills);
        Task<List<CandidateReview>> GetCandidateScreeningHistory();
        Task MoveCandidateToInterviewStage(int candidateId, int positionId);
    }
}
