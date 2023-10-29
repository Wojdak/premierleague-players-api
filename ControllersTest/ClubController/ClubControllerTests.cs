using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PLPlayersAPI.Controllers;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.ClubServices;
using PLPlayersAPI.Services.PositionServices;
using PLPlayersAPI.Validators;

namespace ControllersTests
{
    public class ClubControllerTests
    {
        private readonly IClubService _clubService;
        private readonly ClubController _controller;
        private IValidator<Club> _validator;
        public ClubControllerTests()
        {
            _clubService = A.Fake<IClubService>();
            _validator = new ClubValidator();
            _controller = new ClubController(_clubService, _validator);
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

        [Fact]
        public async Task AddClub_ValidClub_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var club = new Club { ClubId=1, BadgeSrc="Test.png", Name="Test.png" };

            A.CallTo(() => _clubService.AddClubAsync(club)).Returns(club.ClubId);

            // Act
            var result = await _controller.AddClub(club);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal($"Successfully added a new club with id: {club.ClubId}", createdResult.Value);
        }

        [Fact]
        public async Task AddClub_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var club = new Club { ClubId = 1, BadgeSrc = "Test.png", Name = "T" };

            // Act
            var validationResult = await _validator.TestValidateAsync(club);
            var result = await _controller.AddClub(club);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(club => club.Name);
            validationResult.Errors.ForEach(e => e.ErrorMessage.Contains("Minimum length of the country is 5 characters"));
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateClub_ValidClub_ReturnsOkResult()
        {
            // Arrange
            var club = new Club { ClubId = 1, BadgeSrc = "Test.png", Name = "Test.png" };

            A.CallTo(() => _clubService.UpdateClubAsync(club.ClubId, club)).Returns(club.ClubId);

            // Act
            var result = await _controller.UpdateClub(club.ClubId, club);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Successfully updated the club with id: {club.ClubId}", okResult.Value);
        }

        [Fact]
        public async Task UpdateClub_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int invalidClubId = -1;
            var club = new Club { ClubId = 1, BadgeSrc = "Test.png", Name = "TestName" };

            // Act
            var result = await _controller.UpdateClub(invalidClubId, club);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Club with the given Id doesn't exist in the database", notFoundResult.Value);

        }

        [Fact]
        public async Task UpdateClub_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            int existingClubId = 1;
            var club = new Club { ClubId = 1, BadgeSrc = "Test.png", Name = "T" };

            // Act
            var validationResult = await _validator.TestValidateAsync(club);
            var result = await _controller.UpdateClub(existingClubId, club);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(club => club.Name);
            validationResult.Errors.ForEach(e => e.ErrorMessage.Contains("Club length of the position is 3 characters"));
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteClub_ExistingClub_ReturnsNoContentResult()
        {
            // Arrange
            int existingClubId = 1;

            A.CallTo(() => _clubService.DeleteClubAsync(existingClubId)).Returns(true);

            // Act
            var result = await _controller.DeleteClub(existingClubId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteClub_NonExistingClub_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingClubId = 1999;

            A.CallTo(() => _clubService.DeleteClubAsync(nonExistingClubId)).Returns(false);

            // Act
            var result = await _controller.DeleteClub(nonExistingClubId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}