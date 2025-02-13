using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAllPositionsAsync();
        Task<Position> GetPositionByIdAsync(int id);
        Task AddPositionAsync(Position position);
        Task UpdatePositionAsync(Position position);
        Task DeletePositionAsync(int id);
        Task ClosePositionAsync(int positionId, string reasonForClosure, int? linkedCandidateId);
    }
}
