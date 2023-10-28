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

        public async Task<int> AddClubAsync(Club club)
        {
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return club.ClubId;
        }

        public async Task<int?> UpdateClubAsync(int clubId, Club _club)
        {
            var club = await _context.Clubs.FindAsync(clubId);

            if (club == null)
                return null;

            club.Name = _club.Name;
            club.BadgeSrc = _club.BadgeSrc;

            await _context.SaveChangesAsync();
            return club.ClubId;
        }

        public async Task<bool> DeleteClubAsync(int clubId)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubId == clubId);

            if (club is null)
                return false;

            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
