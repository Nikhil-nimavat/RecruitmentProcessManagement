using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IInterviewerService
    {
        Task<List<Interviewer>> GetAllInterviewersAsync();
        Task<Interviewer> GetInterviewerByIdAsync(int id);
        Task AddInterviewerAsync(Interviewer interviewer);
        Task UpdateInterviewerAsync(Interviewer interviewer);
        Task DeleteInterviewerAsync(int id);
    }
}
