using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateReviewService
    {
        Task<bool> AssignReviewer(int positionId, string reviewerId);
        Task<List<Candidate>> GetCandidatesForReview(int positionId);
        Task<bool> SubmitReview(string candidateId, int positionId,
            string reviewerId, string comments,
            string status, List<CandidateSkill> candidateSkills);
        Task<List<CandidateReview>> GetCandidateScreeningHistory(string candidateId);
        Task MoveCandidateToInterviewStage(string candidateId, int positionId);
    }
}
