namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IDocumentVerificationService
    {
       Task VerifyCandidateDocuments(int candidateId, string status, string verifiedBy);
    }
}
