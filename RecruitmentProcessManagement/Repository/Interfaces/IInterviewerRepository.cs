using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IInterviewerRepository
    {
        Task<List<Interviewer>> GetAllAsync();
        Task<Interviewer> GetByIdAsync(int id);
        Task AddAsync(Interviewer interviewer);
        Task UpdateAsync(Interviewer interviewer);
        Task DeleteAsync(int id);
    }
}
