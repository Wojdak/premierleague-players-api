using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLPlayersAPI.Data;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.PositionServices
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PositionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PositionDTO>> GetAllPositionsAsync()
        {
            var positions = await _context.Positions.ToListAsync();

            var positionsDTO = positions.Select(_mapper.Map<Position, PositionDTO>).ToList();

            return positionsDTO;
        }

        public async Task<PositionDTO?> GetPositionByIdAsync(int positionId)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionId == positionId);

            if (position is null) return null;

            var positionDTO = _mapper.Map<Position, PositionDTO>(position);

            return positionDTO;
        }

        public async Task<int> AddPositionAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position.PositionId;
        }

        public async Task<int?> UpdatePositionAsync(int positionId, Position _position)
        {
            var position = await _context.Positions.FindAsync(positionId);

            if (position == null)
                return null;

            position.Name = _position.Name;

            await _context.SaveChangesAsync();
            return position.PositionId;
        }

        public async Task<bool> DeletePositionAsync(int positionId)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.PositionId == positionId);

            if (position is null)
                return false;

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
