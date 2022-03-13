using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public interface ISquaresService
    {
        List<Square> GetSquares(List<Point> input);
    }
}
