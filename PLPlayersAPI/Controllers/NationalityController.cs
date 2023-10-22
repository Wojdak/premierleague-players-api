using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Services.NationalityServices;

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
    }
}
