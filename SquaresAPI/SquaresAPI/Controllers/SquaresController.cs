using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Services;

namespace SquaresAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SquaresController : ControllerBase
    {
        private readonly ISquaresService _squaresService;
        private readonly IPointsService _pointsService;

        public SquaresController(ISquaresService squaresService, IPointsService pointsService)
        {
            _squaresService = squaresService;
            _pointsService = pointsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var squares = _squaresService.GetSquares(await _pointsService.GetAllPoints());
            return Ok(squares);
        }
    }
}
