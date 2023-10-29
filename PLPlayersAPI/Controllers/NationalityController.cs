using FluentValidation;
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
        private IValidator<Nationality> _validator;

        public NationalityController(INationalityService nationalityService, IValidator<Nationality> validator)
        {
            _nationalityService = nationalityService;
            _validator = validator;
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
            var validationResult = await _validator.ValidateAsync(nationality);

            if (validationResult.IsValid)
            {
                var addedNationalityId = await _nationalityService.AddNationalityAsync(nationality);

                return CreatedAtAction(nameof(AddNationality), new { id = addedNationalityId }, $"Successfully added a new nationality with id: {addedNationalityId}");
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> UpdateNationality(int id, [FromBody] Nationality nationality)
        {
            var validationResult = await _validator.ValidateAsync(nationality);

            if (validationResult.IsValid)
            {
                var updatedNationalityId = await _nationalityService.UpdateNationalityAsync(id, nationality);

                if (updatedNationalityId is null)
                    return NotFound("Nationality with the given Id doesn't exist in the database");

                return Ok($"Successfully updated the nationality with id: {updatedNationalityId}");
            }

            return BadRequest(validationResult.Errors);
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
