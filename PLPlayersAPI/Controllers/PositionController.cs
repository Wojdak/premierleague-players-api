using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.PositionServices;

namespace PLPlayersAPI.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private IValidator<Position> _validator;

        public PositionController(IPositionService positionService, IValidator<Position> validator)
        {
            _positionService = positionService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {
            var positions = await _positionService.GetAllPositionsAsync();

            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPositionById(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);

            return position is null ? NotFound("Position with the given Id doesn't exist in the database.") : Ok(position);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> AddPosition([FromBody] Position position)
        {
            var validationResult = await _validator.ValidateAsync(position);

            if (validationResult.IsValid)
            {
                var addedPositionId = await _positionService.AddPositionAsync(position);

                return CreatedAtAction(nameof(AddPosition), new { id = addedPositionId }, $"Successfully added a new position with id: {addedPositionId}");
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] Position position)
        {
            var validationResult = await _validator.ValidateAsync(position);

            if (validationResult.IsValid)
            {
                var updatedPositionId = await _positionService.UpdatePositionAsync(id, position);

                if (updatedPositionId is null)
                    return NotFound("Position with the given Id doesn't exist in the database");

                return Ok($"Successfully updated the position with id: {updatedPositionId}");
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdministratorPolicy")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var deleted = await _positionService.DeletePositionAsync(id);

            if (!deleted)
                return NotFound("Position with the given Id doesn't exist in the database");

            return NoContent();
        }

    }
}
