using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class InterviewerService : IInterviewerService
    {
        private readonly IInterviewerRepository _repository;

        public InterviewerService(IInterviewerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Interviewer>> GetAllInterviewersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Interviewer> GetInterviewerByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddInterviewerAsync(Interviewer interviewer)
        {
            await _repository.AddAsync(interviewer);
        }

        public async Task UpdateInterviewerAsync(Interviewer interviewer)
        {
            await _repository.UpdateAsync(interviewer);
        }

        public async Task DeleteInterviewerAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
