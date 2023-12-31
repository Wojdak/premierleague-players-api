﻿using PLPlayersAPI.Filters;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Wrappers;

namespace PLPlayersAPI.Services.PlayerServices
{
    public interface IPlayerService
    {
        Task<PagedResponse<PlayerDTO>?> GetAllPlayersAsync(PaginationFilter playerFilters, PlayerFilter playerFilter);
        Task<PlayerDTO?> GetPlayerByIdAsync(int playerId);
        Task<int> AddPlayerAsync(Player player);
        Task<int?> UpdatePlayerAsync(int playerId, Player player);
        Task<bool> DeletePlayerAsync(int playerId);
    }
}
