using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Models;
using PLPlayersAPI.Services.NationalityServices;
using System.Numerics;

namespace PLPlayersAPI.Controllers
{
    [Route("api/nationalities")]
    [ApiController]
    public class NationalityController : ControllerBase
    {
        private readonly INationalityService _nationalityService;

        public NationalityController(INationalityService nationalityService)
        {
            _nationalityService = nationalityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNationalities()
        {
            var nationalities = await _nationalityService.GetAllNationalitiesAsync();

            return Ok(nationalities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNationalityById(int id)
        {
            var nationality = await _nationalityService.GetNationalityByIdAsync(id);

            return nationality is null ? NotFound("Nationality with the given Id doesn't exist in the database.") : Ok(nationality);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> AddNationality([FromBody] Nationality nationality)
        {
            var addedNationalityId = await _nationalityService.AddNationalityAsync(nationality);

            return CreatedAtAction(nameof(AddNationality), new { id = addedNationalityId }, $"Successfully added a new nationality with id: {addedNationalityId}");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> UpdateNationality(int id, [FromBody] Nationality nationality)
        {
            var updatedNationalityId = await _nationalityService.UpdateNationalityAsync(id, nationality);

            if (updatedNationalityId is null)
                return NotFound("Nationality with the given Id doesn't exist in the database");

            return Ok($"Successfully updated the nationality with id: {updatedNationalityId}");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> DeleteNationality(int id)
        {
            var deleted = await _nationalityService.DeleteNationalityAsync(id);

            if (!deleted)
                return NotFound("Nationality with the given Id doesn't exist in the database");

            return NoContent();
        }
    }
}
