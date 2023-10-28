using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Controllers;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.PositionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllersTests
{
    public class PositionControllerTests
    {
        private readonly IPositionService _positionService;
        private readonly PositionController _controller;

        public PositionControllerTests()
        {
            _positionService = A.Fake<IPositionService>();
            _controller = new PositionController(_positionService);
        }

        [Fact]
        public async Task GetPositions_ReturnsOkResult()
        {
            //Arange
            var positionsList = A.Fake<List<PositionDTO>>();

            A.CallTo(() => _positionService.GetAllPositionsAsync())
                .Returns(positionsList);

            //Act
            var result = await _controller.GetPositions();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetPositionById_ExistingPosition_ReturnsOkResult()
        {
            // Arrange
            int existingPositionId = 1;
            var existingPosition = new PositionDTO { PositionId = existingPositionId, Name = "Forward" };

            A.CallTo(() => _positionService.GetPositionByIdAsync(existingPositionId))
                .Returns(existingPosition);

            //Act
            var result = await _controller.GetPositionById(existingPositionId);

            //Arrange
            var okResult = Assert.IsType<OkObjectResult>(result);
            var position = Assert.IsType<PositionDTO>(okResult.Value);
            Assert.Equal(existingPositionId, position.PositionId);
            Assert.Equal(existingPosition.Name, position.Name);
        }

        [Fact]
        public async Task GetPositionById_NonExistingPosition_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingPositionId = 2;
            A.CallTo(() => _positionService.GetPositionByIdAsync(nonExistingPositionId))
                .Returns((PositionDTO)null);

            //Act
            var result = await _controller.GetPositionById(nonExistingPositionId);

            //Arrange
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Position with the given Id doesn't exist in the database.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddPosition_ValidPosition_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var position = new Position { PositionId = 1, Name = "test" };

            A.CallTo(() => _positionService.AddPositionAsync(position)).Returns(position.PositionId);

            // Act
            var result = await _controller.AddPosition(position);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal($"Successfully added a new position with id: {position.PositionId}", createdResult.Value);
        }

        [Fact]
        public async Task UpdatePosition_ValidPosition_ReturnsOkResult()
        {
            // Arrange
            var position = new Position { PositionId = 1, Name = "test" };

            A.CallTo(() => _positionService.UpdatePositionAsync(position.PositionId, position)).Returns(position.PositionId);

            // Act
            var result = await _controller.UpdatePosition(position.PositionId, position);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Successfully updated the position with id: {position.PositionId}", okResult.Value);
        }

        [Fact]
        public async Task DeletePosition_ExistingPosition_ReturnsNoContentResult()
        {
            // Arrange
            int existingPositionId = 1;

            A.CallTo(() => _positionService.DeletePositionAsync(existingPositionId)).Returns(true);

            // Act
            var result = await _controller.DeletePosition(existingPositionId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
