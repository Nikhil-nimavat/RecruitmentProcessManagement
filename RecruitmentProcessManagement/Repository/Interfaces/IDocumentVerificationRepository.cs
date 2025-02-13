namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IDocumentVerificationRepository
    {
        Task VerifyCandidateDocuments(int candidateId, string status, string verifiedBy);
    }
}
