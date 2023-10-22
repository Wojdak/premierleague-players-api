using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Controllers;
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
    }
}
