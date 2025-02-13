using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;
using System.Linq.Expressions;
using System.Collections;

namespace RecruitmentProcessManagement.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly Data.ApplicationDbContext _context;

        public PositionRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            List<Position> positions = await _context.Positions.ToListAsync();
            return positions;
        }

        public async Task<Position> GetPositionByIdAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            return position;
        }

        public async Task AddPositionAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePositionAsync(Position position)
        {
            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePositionAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ClosePositionAsync(int positionId, string reasonForClosure, int? linkedCandidateId)
        {
            var position = await _context.Positions.FindAsync(positionId);
            if (position == null) throw new Exception("Position not found");

            position.Status = "Closed";
            position.ReasonForClosure = reasonForClosure;
            position.PositionClosedDate = DateTime.Now;

            if (linkedCandidateId != null)
            {
                position.LinkedCandidateID = linkedCandidateId;
            }

            await _context.SaveChangesAsync();
        }


    }
}
