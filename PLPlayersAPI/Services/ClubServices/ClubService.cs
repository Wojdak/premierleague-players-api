using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLPlayersAPI.Data;
using PLPlayersAPI.Filters;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Wrappers;

namespace PLPlayersAPI.Services.ClubServices
{
    public class ClubService : IClubService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClubService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClubDTO>> GetAllClubsAsync()
        {
            var clubs = await _context.Clubs.ToListAsync();

            var clubsDTO = clubs.Select(_mapper.Map<Club, ClubDTO>).ToList();

            return clubsDTO;
        }

        public async Task<ClubDTO?> GetClubByIdAsync(int clubId)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(c=>c.ClubId == clubId);

            if (club is null) return null;

            var clubDTO = _mapper.Map<Club, ClubDTO>(club);

            return clubDTO;
        }
    }
}
