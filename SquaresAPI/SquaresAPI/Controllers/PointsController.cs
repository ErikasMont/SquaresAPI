using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Models;
using SquaresAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SquaresAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IPointsService _service;

        public PointsController(IPointsService service)
        {
                _service = service;
        }

        // POST api/<PointsController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] Point point)
        {
            if(!ModelState.IsValid || point == null)
            {
                return BadRequest();
            }
            var points = await _service.GetAllPoints();
            var exists = points.Any(x => x.X == point.X && x.Y == point.Y);
            if (exists)
            {
                return BadRequest();
            }
            else
            {
                await _service.AddPoint(point);
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        // POST api/<PointsController>/importpoints
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImportPoints([FromBody] List<Point> points)
        {
            if (points.Count == 0)
            {
                return BadRequest();
            }
            else
            {
                await _service.ImportPoints(points);
                return StatusCode(StatusCodes.Status201Created);
            }
        }


        // DELETE api/<PointsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            var point = await _service.GetPointById(id);
            if(point == null)
            {
                return BadRequest();
            }
            else
            {
                await _service.RemovePoint(point);
                return Ok();
            }
        }
    }
}
