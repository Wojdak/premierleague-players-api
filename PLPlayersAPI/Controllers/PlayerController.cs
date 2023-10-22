using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLPlayersAPI.Filters;
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

            if(players is null) return NotFound("No data matching the request was found in the database.");

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

            return player is null ? NotFound("Player with the given Id doesn't exist in the database.") : Ok(player);
        }



    }
}
