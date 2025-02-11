using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly ApplicationDbContext _context;
        public DocumentVerificationService(ApplicationDbContext context) 
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
