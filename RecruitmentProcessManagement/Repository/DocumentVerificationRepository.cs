using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class DocumentVerificationRepository: IDocumentVerificationRepository
    {
        private readonly Data.ApplicationDbContext _context;
        public DocumentVerificationRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task VerifyCandidateDocuments(int candidateId, string status, string verifiedBy)
        {
            var verification = new DocumentVerification
            {
                CandidateID = candidateId,
                VerificationStatus = status,
                VerificationDate = DateTime.Now,
                VerifiedBy = verifiedBy
            };

            await _context.DocumentVerifications.AddAsync(verification);
            await _context.SaveChangesAsync();
        }
    }
}
