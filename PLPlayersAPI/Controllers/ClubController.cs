using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Models;
using PLPlayersAPI.Services.ClubServices;

namespace PLPlayersAPI.Controllers
{
    [Route("api/clubs")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClubs()
        {
            var clubs = await _clubService.GetAllClubsAsync();

            return Ok(clubs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClubById(int id)
        {
            var club = await _clubService.GetClubByIdAsync(id);

            return club is null ? NotFound("Club with the given Id doesn't exist in the database.") : Ok(club);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> AddClub([FromBody] Club club)
        {
            var addedClubId = await _clubService.AddClubAsync(club);

            return CreatedAtAction(nameof(AddClub), new { id = addedClubId }, $"Successfully added a new club with id: {addedClubId}");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> UpdateClub(int id, [FromBody] Club club)
        {
            var updatedClubId = await _clubService.UpdateClubAsync(id, club);

            if (updatedClubId is null)
                return NotFound("Club with the given Id doesn't exist in the database");

            return Ok($"Successfully updated the club with id: {updatedClubId}");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var deleted = await _clubService.DeleteClubAsync(id);

            if (!deleted)
                return NotFound("Club with the given Id doesn't exist in the database");

            return NoContent();
        }

    }
}
