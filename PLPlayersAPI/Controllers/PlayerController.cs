using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLPlayersAPI.Filters;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.PlayerServices;

namespace PLPlayersAPI.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers([FromQuery] PaginationFilter paginationFilters, [FromQuery] PlayerFilter playerFilter)
        {
            var players = await _playerService.GetAllPlayersAsync(paginationFilters, playerFilter);

            if(players is null) return NotFound("No data matching the request was found in the database");

            var metadata = new 
            {
                players.CurrentPage,
                players.PageSize,
                players.TotalRecords,
                players.TotalPages,
                players.HasPrevious,
                players.HasNext,
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            return player is null ? NotFound("Player with the given Id doesn't exist in the database") : Ok(player);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> AddPlayer([FromBody] Player player)
        {
            var addedPlayerId = await _playerService.AddPlayerAsync(player);

            return CreatedAtAction(nameof(GetPlayerById), new { id = addedPlayerId }, $"Successfully added a new player with id: {addedPlayerId}");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player player)
        {
            var updatedPlayerId = await _playerService.UpdatePlayerAsync(id, player);

            if (updatedPlayerId is null)
                return NotFound("Player with the given Id doesn't exist in the database");

            return Ok($"Successfully updated the player with id: {updatedPlayerId}");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var deleted = await _playerService.DeletePlayerAsync(id);

            if (!deleted)
                return NotFound("Player with the given Id doesn't exist in the database");

            return NoContent();
        }

    }
}
