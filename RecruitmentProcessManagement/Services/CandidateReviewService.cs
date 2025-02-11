using DocumentFormat.OpenXml.Spreadsheet;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class CandidateReviewService : ICandidateReviewService
    {
        private readonly ICandidateReviewRepository _reviewRepository;
        private readonly ApplicationDbContext _context;

        public CandidateReviewService(ICandidateReviewRepository reviewRepository, ApplicationDbContext context)
        {
            _reviewRepository = reviewRepository;
            _context = context;
        }

        public async Task<bool> AssignReviewer(int positionId, int reviewerId)
        {
            return await _reviewRepository.AssignReviewer(positionId, reviewerId);
        }

        public async Task<List<Candidate>> GetCandidatesForReview(int positionId)
        {
            return await _reviewRepository.GetCandidatesForReview(positionId);
        }

        public async Task<bool> SubmitReview(int candidateId, int positionId,
            int reviewerId, string comments,
            string status, List<CandidateSkill> candidateSkills)
        {
            var review = new CandidateReview
            {
                CandidateID = candidateId,
                PositionID = positionId,
                ReviewerID = reviewerId.ToString(),
                Comments = comments,
                Status = status,
                ReviewDate = DateTime.Now
            };

            _context.CandidateReviews.Add(review);
            await _context.SaveChangesAsync();

            if (status == "Shortlisted")
            {
                await MoveCandidateToInterviewStage(candidateId, positionId);
            }

            return true;
        }

        public async Task MoveCandidateToInterviewStage(int candidateId, int positionId)
        {
            var interview = new Interview
            {
                CandidateID = candidateId,
                PositionID = positionId,
                InterviewDate = DateTime.Now.AddDays(2),
                InterviewerID = 1
            };

            _context.Interviews.Add(interview);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CandidateReview>> GetCandidateScreeningHistory(int candidateId)
        {
            return await _reviewRepository.GetCandidateScreeningHistory(candidateId);
        }

    }
}
