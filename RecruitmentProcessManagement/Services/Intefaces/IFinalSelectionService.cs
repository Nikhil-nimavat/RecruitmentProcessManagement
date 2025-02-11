namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IFinalSelectionService
    {
        Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate);
    }
}
