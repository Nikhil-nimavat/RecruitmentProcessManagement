using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        public PositionService(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            return await _repository.GetAllPositionsAsync();
        }

        public async Task<Position> GetPositionByIdAsync(int id)
        {
            return await _repository.GetPositionByIdAsync(id);
        }

        public async Task AddPositionAsync(Position position)
        {
            await _repository.AddPositionAsync(position);
        }

        public async Task UpdatePositionAsync(Position position)
        {
            await _repository.UpdatePositionAsync(position);
        }

        public async Task DeletePositionAsync(int id)
        {
            await _repository.DeletePositionAsync(id);
        } 
        public async Task ClosePositionAsync(int positionId, string reasonForClosure, int? linkedCandidateId)
        {
           await _repository.ClosePositionAsync(positionId, reasonForClosure, linkedCandidateId);   
        }

    }
}
