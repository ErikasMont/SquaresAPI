using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Controllers;
using SquaresAPI.Data;
using SquaresAPI.Models;
using SquaresAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SquaresAPIUnitTest
{
    public class PointsControllerTests
    {
        private readonly DbContextOptions<ApiDbContext> _dbContextOptions;
        private readonly ApiDbContext _dbContext;
        private readonly IPointsService _service;
        private readonly PointsController _controller;

        public PointsControllerTests()
        {
            var dbName = $"PointsDbTesting_{DateTime.Now.ToFileTimeUtc()}";
            _dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            _dbContext = new ApiDbContext(_dbContextOptions);
            _service = new PointsService(_dbContext);
            _controller = new PointsController(_service);
        }


        [Fact]
        public async Task Post_WhenCalled_ReturnsCreatedResponse()
        {
            //Arrange
            await PopulateData();
            Point point = new Point()
            {
                X = 2,
                Y = 3
            };

            //Act
            var createdResponse = await _controller.Post(point) as StatusCodeResult;

            //Assert
            Assert.IsType<StatusCodeResult>(createdResponse);
            Assert.Equal(201, createdResponse.StatusCode);
        }

        [Fact]
        public async Task Post_WhenCalled_PointAddedToDb()
        {
            //Arrange
            await PopulateData();
            Point point = new Point()
            {
                X = 2,
                Y = 3
            };

            //Act
            await _controller.Post(point);

            //Assert
            Assert.Equal(13, await _dbContext.Points.CountAsync());
        }

        [Fact]
        public async Task Post_WhenDuplicateIsGiven_ReturnsBadRequestResponse()
        {
            //Arrange
            await PopulateData();
            Point point = new Point()
            {
                X = 1,
                Y = 1
            };

            //Act
            var badRequest = await _controller.Post(point) as BadRequestResult;

            //Assert
            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public async Task Post_WhenPointIsNull_ReturnsBadRequestResponse()
        {
            //Arrange
            await PopulateData();
            Point point = null;

            //Act
            var badRequest = await _controller.Post(point) as BadRequestResult;

            //Assert
            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public async Task ImportPoints_WhenCalled_ReturnsCreatedResponse()
        {
            //Arrange
            await PopulateData();
            List<Point> points = new List<Point>()
            {
                new Point(){ X = 0, Y = 0},
                new Point(){ X = 5, Y = 5},
                new Point(){ X = -7, Y = -3}
            };

            //Act
            var createdResponse = await _controller.ImportPoints(points) as StatusCodeResult;
            Assert.IsType<StatusCodeResult>(createdResponse);
            Assert.Equal(201, createdResponse.StatusCode);
        }

        [Fact]
        public async Task ImportPoints_WhenCalled_PointsAddedToDb()
        {
            //Arrange
            await PopulateData();
            List<Point> points = new List<Point>()
            {
                new Point(){ X = 0, Y = 0},
                new Point(){ X = 5, Y = 5},
                new Point(){ X = -7, Y = -3}
            };

            // Act
            await _controller.ImportPoints(points);

            //Assert
            Assert.Equal(15, await _dbContext.Points.CountAsync());
        }

        [Fact]
        public async Task ImportPoints_WhenDuplicatesGiven_OnlyNonDuplicatesAddedToDb()
        {
            //Arrange
            await PopulateData();
            List<Point> points = new List<Point>()
            {
                new Point(){ X = 0, Y = 0},
                new Point(){ X = 1, Y = 1},
                new Point(){ X = -2, Y = -2}
            };

            // Act
            await _controller.ImportPoints(points);

            //Assert
            Assert.Equal(13, await _dbContext.Points.CountAsync());
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsOkResponse()
        {
            //Arrange
            await PopulateData();
            int id = 5;

            //Act
            var okResponse = await _controller.Delete(id) as OkResult;

            //Assert
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public async Task Delete_WhenCalled_DeletesPointFromDb()
        {
            //Arrange
            await PopulateData();
            int id = 5;

            //Act
            await _controller.Delete(id);

            //Assert
            Assert.Equal(11, await _dbContext.Points.CountAsync());
        }

        [Fact]
        public async Task Delete_WhenBadIdGiven_ReturnsBadRequestResponse()
        {
            //Arrange
            await PopulateData();
            int id = 20;

            //Act
            var badRequest = await _controller.Delete(id) as BadRequestResult;

            //Assert
            Assert.IsType<BadRequestResult>(badRequest);
        }

        private async Task PopulateData()
        {
            List<Point> points = new List<Point>()
            {
                new Point(){ X = -1, Y = 1 },
                new Point(){ X = 1, Y = 1 },
                new Point(){ X = 4, Y = 9},
                new Point(){ X = -2, Y = 2},
                new Point(){ X = 5, Y = -3},
                new Point(){ X = 1, Y = -1},
                new Point(){ X = 2, Y = 2},
                new Point(){ X = 9, Y = 1},
                new Point(){ X = -1, Y = -1},
                new Point(){ X = 2, Y = -2},
                new Point(){ X = 6, Y = 0},
                new Point(){ X = -2, Y = -2}
            };

            await _controller.ImportPoints(points);
        }
    }
}