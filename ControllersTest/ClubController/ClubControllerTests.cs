using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Controllers;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.ClubServices;

namespace ControllersTests
{
    public class ClubControllerTests
    {
        private readonly IClubService _clubService;
        private readonly ClubController _controller;

        public ClubControllerTests()
        {
            _clubService = A.Fake<IClubService>();
            _controller = new ClubController(_clubService);
        }

        [Fact]
        public async Task GetClubs_ReturnsOkResult()
        {
            // Arrange
            var clubsList = A.Fake<List<ClubDTO>>();

            A.CallTo(() => _clubService.GetAllClubsAsync())
                .Returns(clubsList);

            // Act
            var result = await _controller.GetClubs(); 

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetClubById_ExistingClub_ReturnsOkResult()
        {
            // Arrange
            int existingClubId = 1;
            var existingClub = new ClubDTO { ClubId = existingClubId, Name = "Test Club" };

            A.CallTo(() => _clubService.GetClubByIdAsync(existingClubId))
                .Returns(existingClub);

            // Act
            var result = await _controller.GetClubById(existingClubId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var club = Assert.IsType<ClubDTO>(okResult.Value);
            Assert.Equal(existingClubId, club.ClubId);
        }

        [Fact]
        public async Task GetClubById_NonExistingClub_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingClubId = 2;

            A.CallTo(() => _clubService.GetClubByIdAsync(nonExistingClubId))
                .Returns((ClubDTO)null);

            // Act
            var result = await _controller.GetClubById(nonExistingClubId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Club with the given Id doesn't exist in the database.", notFoundResult.Value);
        }
    }
}