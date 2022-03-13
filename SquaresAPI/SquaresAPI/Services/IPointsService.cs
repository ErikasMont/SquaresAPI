using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public interface IPointsService
    {
        Task<List<Point>> GetAllPoints();
        Task<Point> GetPointById(int id);
        Task AddPoint(Point point);
        Task ImportPoints(List<Point> points);
        Task RemovePoint(Point point);
    }
}
