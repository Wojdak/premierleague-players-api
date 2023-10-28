﻿using PLPlayersAPI.Services.NationalityServices;
using PLPlayersAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLPlayersAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Models;

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

        [Fact]
        public async Task AddNationality_ValidNationality_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var nationality = new Nationality
            {
                NationalityId = 1,
                Country = "Test",
                FlagSrc = "Test.png"
            };

            A.CallTo(() => _nationalityService.AddNationalityAsync(nationality)).Returns(nationality.NationalityId);

            // Act
            var result = await _controller.AddNationality(nationality);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal($"Successfully added a new nationality with id: {nationality.NationalityId}", createdResult.Value);
        }


        [Fact]
        public async Task UpdateNationality_ValidNationality_ReturnsOkResult()
        {
            // Arrange
            var nationality = new Nationality
            {
                NationalityId = 1,
                Country = "Test",
                FlagSrc = "Test.png"
            };

            A.CallTo(() => _nationalityService.UpdateNationalityAsync(nationality.NationalityId, nationality)).Returns(nationality.NationalityId);

            // Act
            var result = await _controller.UpdateNationality(nationality.NationalityId, nationality);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Successfully updated the nationality with id: {nationality.NationalityId}", okResult.Value);
        }

        [Fact]
        public async Task DeleteNationality_ExistingNationality_ReturnsNoContentResult()
        {
            // Arrange
            int existingNationalityId = 1;

            A.CallTo(() => _nationalityService.DeleteNationalityAsync(existingNationalityId)).Returns(true);

            // Act
            var result = await _controller.DeleteNationality(existingNationalityId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
