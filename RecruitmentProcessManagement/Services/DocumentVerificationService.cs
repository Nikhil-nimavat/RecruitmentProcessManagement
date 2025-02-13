using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly IDocumentVerificationRepository _repository;
        public DocumentVerificationService(IDocumentVerificationRepository repository) 
        {
            _repository = repository;
        }

        public async Task VerifyCandidateDocuments(int candidateId, string status, string verifiedBy)
        {
            await _repository.VerifyCandidateDocuments(candidateId, status, verifiedBy);    
        }
    }
}
