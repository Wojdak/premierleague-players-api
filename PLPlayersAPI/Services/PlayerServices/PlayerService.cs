using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PLPlayersAPI.Data;
using PLPlayersAPI.Filters;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Wrappers;

namespace PLPlayersAPI.Services.PlayerServices
{
    public class PlayerService : IPlayerService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlayerService(AppDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PlayerDTO>?> GetAllPlayersAsync(PaginationFilter paginationFilters, PlayerFilter playerFilter)
        {
            var filteredQuery = BuildFilteredQuery(playerFilter);
            if (filteredQuery is null) return null;

            var validPagination = new PaginationFilter(paginationFilters.PageNumber, paginationFilters.PageSize);

            var players = await filteredQuery.ToListAsync();
            var playerDTOs = players.Select(_mapper.Map<Player, PlayerDTO>).ToList();

            return new PagedResponse<PlayerDTO>(playerDTOs, playerDTOs.Count, validPagination.PageNumber, validPagination.PageSize);
        }

        private IQueryable<Player> BuildFilteredQuery(PlayerFilter playerFilter)
        {
            var query = _context.Players
                .Include(p => p.Nationality)
                .Include(p => p.Club)
                .Include(p => p.Position)
                .AsQueryable();

            if (!string.IsNullOrEmpty(playerFilter.Country))
                query = query.Where(p => p.Nationality.Country == playerFilter.Country);

            if (!string.IsNullOrEmpty(playerFilter.Club))
                query = query.Where(p => p.Club.Name == playerFilter.Club);

            if (!string.IsNullOrEmpty(playerFilter.Position))
                query = query.Where(p => p.Position.Name == playerFilter.Position);

            return query.Any() ? query : null;
        }


        public async Task<PlayerDTO?> GetPlayerByIdAsync(int playerId)
        {
            var player = await _context.Players
                .Include(p => p.Club)
                .Include(p => p.Nationality)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(p => p.PlayerId == playerId);

            if (player is null) return null; 

            var playerDTO = _mapper.Map<Player, PlayerDTO>(player);

            return playerDTO;
        }

        public async Task<int?> AddPlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return player.PlayerId;
        }

        public async Task<int?> UpdatePlayerAsync(int playerId, Player _player)
        {
            var player = await _context.Players.FindAsync(playerId);

            if (player == null)
                return null; 

            player.FirstName = _player.FirstName;
            player.LastName = _player.LastName;
            player.ImgSrc = _player.ImgSrc;
            player.DateOfBirth = _player.DateOfBirth; 
            player.ClubId = _player.ClubId;
            player.NationalityId = _player.NationalityId;
            player.PositionId = _player.PositionId;


            await _context.SaveChangesAsync();
            return player.PlayerId;
        }

        public async Task<bool> DeletePlayerAsync(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);

            if (player == null)
                return false; 

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
