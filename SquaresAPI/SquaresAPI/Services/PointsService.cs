using Microsoft.EntityFrameworkCore;
using SquaresAPI.Data;
using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public class PointsService : IPointsService
    {
        private readonly ApiDbContext _dbContext;
        public PointsService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddPoint(Point point)
        {
            await _dbContext.Points.AddAsync(point);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportPoints(List<Point> points)
        {
            foreach (var point in points)
            {
                if (point == null || await _dbContext.Points.AnyAsync(x => x.X == point.X && x.Y == point.Y))
                {
                    continue;
                }
                await _dbContext.Points.AddAsync(point);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Point>> GetAllPoints()
        {
            return await _dbContext.Points.ToListAsync();
        }

        public async Task<Point> GetPointById(int id)
        {
            return await _dbContext.Points.FindAsync(id);
        }

        public async Task RemovePoint(Point point)
        {
            _dbContext.Points.Remove(point);
            await _dbContext.SaveChangesAsync();
        }
    }
}
