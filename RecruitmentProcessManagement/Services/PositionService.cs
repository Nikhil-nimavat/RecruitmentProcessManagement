﻿using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class PositionService : IPositionService
    {
        private readonly ApplicationDbContext _context;
        public PositionService(ApplicationDbContext context)
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
    }
}
