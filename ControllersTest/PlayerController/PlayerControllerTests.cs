using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure;
using FakeItEasy.Sdk;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PLPlayersAPI.Controllers;
using PLPlayersAPI.Filters;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.PlayerServices;
using PLPlayersAPI.Wrappers;
using PLPlayersAPI.Models;

namespace ControllersTests
{
    public class PlayerControllerTests
    {
        private readonly IPlayerService _playerService;
        private readonly PlayerController _controller;
    
        public PlayerControllerTests()
        {
            _playerService = A.Fake<IPlayerService>();
            _controller = new PlayerController(_playerService);
         
        }

        [Fact]
        public async Task GetPlayers_ValidFilters_ReturnsOkResultWithPaginationHeaders()
        {
            // Arrange
            var paginationFilters = new PaginationFilter { PageNumber = 1, PageSize = 5 };
            var playerFilter = new PlayerFilter {Position=null, Club=null, Country = "Italy" };
            var playersDto = new List<PlayerDTO> { 
                new PlayerDTO {
                    PlayerId=1, 
                    FirstName="Test", 
                    LastName="Name", 
                    Club = new ClubDTO { ClubId=1, Name="Test Club", BadgeSrc="Test link.png" }, 
                    DateOfBirth="1999-05-23",
                    ImgSrc="Test image.png",
                    Nationality= new NationalityDTO {NationalityId=1, Country="Italy", FlagSrc="Test link.png"},
                    Position = new PositionDTO {PositionId=1, Name="Forward"}
                    } 
                };

            var pagedResponse = new PagedResponse<PlayerDTO>(playersDto, playersDto.Count, paginationFilters.PageNumber, paginationFilters.PageSize);

            A.CallTo(() => _playerService.GetAllPlayersAsync(paginationFilters, playerFilter))
                .Returns(pagedResponse);

            //Arrange dummy headers for the response
            var httpContext = A.Fake<HttpContext>();
            var httpResponse = A.Fake<HttpResponse>();
            A.CallTo(() => httpContext.Response).Returns(httpResponse);

            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };


            // Act
            var result = await _controller.GetPlayers(paginationFilters, playerFilter);

           
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var players = Assert.IsType <PagedResponse<PlayerDTO>> (okResult.Value);
        }

        [Fact]
        public async Task GetPlayers_NoPlayers_ReturnsNotFoundResult()
        {
            // Arrange
            var paginationFilters = new PaginationFilter { PageNumber = 1, PageSize = 10 };
            var playerFilter = new PlayerFilter();

            A.CallTo(() => _playerService.GetAllPlayersAsync(paginationFilters, playerFilter))
                .Returns((PagedResponse<PlayerDTO>)null);

            // Act
            var result = await _controller.GetPlayers(paginationFilters, playerFilter);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No data matching the request was found in the database.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPlayerById_ExistingPlayer_ReturnsOkResult()
        {
            // Arrange
            int existingPlayerId = 1;
            var existingPlayer = new PlayerDTO
            {
                PlayerId = 1,
                FirstName = "Test",
                LastName = "Name",
                Club = new ClubDTO { ClubId = 1, Name = "Test Club", BadgeSrc = "Test link.png" },
                DateOfBirth = "1999-05-23",
                ImgSrc = "Test image.png",
                Nationality = new NationalityDTO { NationalityId = 1, Country = "Italy", FlagSrc = "Test link.png" },
                Position = new PositionDTO { PositionId = 1, Name = "Forward" }
            };

            A.CallTo(() => _playerService.GetPlayerByIdAsync(existingPlayerId))
                .Returns(existingPlayer);

            // Act
            var result = await _controller.GetPlayerById(existingPlayerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var player = Assert.IsType<PlayerDTO>(okResult.Value);
            Assert.Equal(existingPlayerId, player.PlayerId);
        }

        [Fact]
        public async Task GetPlayerById_NonExistingPlayer_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistingPlayerId = 2;

            A.CallTo(() => _playerService.GetPlayerByIdAsync(nonExistingPlayerId))
                .Returns((PlayerDTO)null);

            // Act
            var result = await _controller.GetPlayerById(nonExistingPlayerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Player with the given Id doesn't exist in the database.", notFoundResult.Value);
        }
    }
}
