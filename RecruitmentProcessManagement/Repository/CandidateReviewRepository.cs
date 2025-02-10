using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class CandidateReviewRepository : ICandidateReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Assign a reviewer to a position
        public async Task<bool> AssignReviewer(int positionId, string reviewerId)
        {
            var position = await _context.Positions.FindAsync(positionId);
            if (position == null || position.Status == "Closed")
                return false;

            var existingReview = await _context.CandidateReviews
                .FirstOrDefaultAsync(r => r.PositionID == positionId && r.ReviewerID == reviewerId);

            if (existingReview == null)
            {
                var review = new CandidateReview
                {
                    PositionID = positionId,
                    ReviewerID = reviewerId,
                    ReviewDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.CandidateReviews.Add(review);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        // Get candidates assigned for review
        public async Task<List<Candidate>> GetCandidatesForReview(int positionId)
        {
            return await _context.Candidates
                .Where(c => _context.CandidateReviews.Any(r => r.PositionID == positionId && r.CandidateID == c.CandidateID))
                .ToListAsync();
        }

        // Get an existing review
        public async Task<CandidateReview> GetReview(string candidateId, int positionId, string reviewerId)
        {
            return await _context.CandidateReviews
                .FirstOrDefaultAsync(r => r.CandidateID == candidateId && r.PositionID == positionId && r.ReviewerID == reviewerId);
        }

        // Submit a review
        public async Task<bool> SubmitReview(CandidateReview review, Candidate candidate, List<CandidateSkill> skills)
        {
            if (review == null || candidate == null)
                return false;

            // Save selected skills
            foreach (var skill in skills)
            {
                var existingSkill = await _context.CandidateSkills
                    .FirstOrDefaultAsync(cs => cs.CandidateID == candidate.CandidateID && cs.SkillID == skill.SkillID);

                if (existingSkill == null)
                {
                    _context.CandidateSkills.Add(new CandidateSkill
                    {
                        CandidateID = candidate.CandidateID,
                        SkillID = skill.SkillID,
                        YearsOfExperience = skill.YearsOfExperience
                    });
                }
                else
                {
                    existingSkill.YearsOfExperience = skill.YearsOfExperience;
                }
            }

            _context.CandidateReviews.Update(review);
            _context.Candidates.Update(candidate);

            // If shortlisted, move to interview stage
            if (review.Status == "Shortlisted")
            {
                _context.Interviews.Add(new Interview
                {
                    CandidateID = candidate.CandidateID,
                    PositionID = review.PositionID,
                    InterviewDate = DateTime.Now.AddDays(3), // Default to 3 days later
                    InterviewType = "Technical",
                    Status = "Scheduled"
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Get screening history for a candidate
        public async Task<List<CandidateReview>> GetCandidateScreeningHistory(string candidateId)
        {
            return await _context.CandidateReviews
                .Where(r => r.CandidateID == candidateId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();
        }
    }
}
