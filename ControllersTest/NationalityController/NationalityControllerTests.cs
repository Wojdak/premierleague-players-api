using PLPlayersAPI.Services.NationalityServices;
using PLPlayersAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLPlayersAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ControllersTests
{
    public class NationalityControllerTests
    {
        private readonly INationalityService _nationalityService;
        private readonly NationalityController _controller;
        public NationalityControllerTests()
        {
            _nationalityService = A.Fake<INationalityService>();
            _controller = new NationalityController(_nationalityService);
        }

        [Fact]
        public async Task GetNationalities_ReturnsOkResult()
        {
            //Arange
            var nationalitiesList = A.Fake<List<NationalityDTO>>();

            A.CallTo(()=>_nationalityService.GetAllNationalitiesAsync())
                .Returns(nationalitiesList);

            //Act
            var result = await _controller.GetNationalities();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetNationalityById_ExistingNationality_ReturnsOkResult()
        {
            // Arrange
            int existingNationalityId = 1;
            var existingNationality = new NationalityDTO { NationalityId = existingNationalityId, Country = "Test Country", FlagSrc="Test image link.png" };
            A.CallTo(() => _nationalityService.GetNationalityByIdAsync(existingNationalityId))
                .Returns(existingNationality);

            //Act
            var result = await _controller.GetNationalityById(existingNationalityId);

            //Arrange
            var okResult = Assert.IsType<OkObjectResult>(result);
            var nationality = Assert.IsType<NationalityDTO>(okResult.Value);
            Assert.Equal(existingNationalityId, nationality.NationalityId);
            Assert.Equal(existingNationality.Country, nationality.Country);
            Assert.Equal(existingNationality.FlagSrc, nationality.FlagSrc);
        }

        [Fact]
        public async Task GetNationalityById_NonExistingNationality_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingNationalityId = 2;
            A.CallTo(() => _nationalityService.GetNationalityByIdAsync(nonExistingNationalityId))
                .Returns((NationalityDTO)null);

            //Act
            var result = await _controller.GetNationalityById(nonExistingNationalityId);

            //Arrange
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Nationality with the given Id doesn't exist in the database.", notFoundResult.Value);
        }
    }
}
