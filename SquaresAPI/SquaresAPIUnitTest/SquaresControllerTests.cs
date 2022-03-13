using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Controllers;
using SquaresAPI.Data;
using SquaresAPI.Models;
using SquaresAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SquaresAPIUnitTest
{
    public class SquaresControllerTests
    {
        private readonly DbContextOptions<ApiDbContext> _dbContextOptions;
        private readonly ApiDbContext _dbContext;
        private readonly ISquaresService _squaresService;
        private readonly IPointsService _pointsService;
        private readonly SquaresController _squaresController;
        private readonly PointsController _pointsController;

        public SquaresControllerTests()
        {
            var dbName = $"SquaresDbTesting_{DateTime.Now.ToFileTimeUtc()}";
            _dbContextOptions = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            _dbContext = new ApiDbContext(_dbContextOptions);
            _squaresService = new SquaresService(_dbContext);
            _pointsService = new PointsService(_dbContext);
            _squaresController = new SquaresController(_squaresService, _pointsService);
            _pointsController = new PointsController(_pointsService);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResponse()
        {
            //Arrange
            await PopulateData();

            //Act
            var okResponse = await _squaresController.Get() as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(okResponse);
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsListOfSquares()
        {
            //Arrange
            await PopulateData();

            //Act
            var squares = await _squaresController.Get() as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(squares);
            Assert.Equal(2, (squares.Value as List<Square>).Count);
        }

        [Fact]
        public async Task Get_WhenThereAreNoSquares_ReturnsEmptyList()
        {
            //Arrange
            List<Point> points = new List<Point>()
            {
                new Point(){ X = 5, Y = -8},
                new Point(){ X = 4, Y = -9},
                new Point(){ X = 8, Y = 0},
                new Point(){ X = 0, Y = 0},
                new Point(){ X = 10, Y = -1},
                new Point(){ X = -9, Y = -8},
            };
            await _pointsController.ImportPoints(points);

            //Act 
            var squares = await _squaresController.Get() as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(squares);
            Assert.Equal(0, (squares.Value as List<Square>).Count);
        }

        [Fact]
        public async Task Get_WhenThreeOrLessPointsGiven_ReturnsEmptyList()
        {
            //Arrange
            List<Point> points = new List<Point>()
            {
                new Point(){ X = 5, Y = -8},
                new Point(){ X = 4, Y = -9},
                new Point(){ X = 8, Y = 0}
            };
            await _pointsController.ImportPoints(points);

            //Act 
            var squares = await _squaresController.Get() as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(squares);
            Assert.Equal(0, (squares.Value as List<Square>).Count);
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

            await _pointsController.ImportPoints(points);
        }
    }
}
