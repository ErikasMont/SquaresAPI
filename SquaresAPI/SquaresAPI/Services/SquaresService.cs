using SquaresAPI.Data;
using SquaresAPI.Models;

namespace SquaresAPI.Services
{
    public class SquaresService : ISquaresService
    {
        private readonly ApiDbContext _dbContext;
        public SquaresService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Square> GetSquares(List<Point> input)
        {
            List<Square> squares = new List<Square>();

            if(input.Count <= 3)
            {
                return squares;
            }

            for (int i = 0; i < input.Count - 3; i++)
            {
                for (int j = i + 1; j < input.Count - 2; j++)
                {
                    for (int k = j + 1; k < input.Count - 1; k++)
                    {
                        for (int m = k + 1; m < input.Count; m++)
                        {
                            int distB = DistSq(input[i], input[j]); // from p1 to p2
                            int distC = DistSq(input[i], input[k]); // from p1 to p3
                            int distD = DistSq(input[i], input[m]); // from p1 to p4

                            if (distB == 0 || distC == 0 || distD == 0)
                            {
                                continue;
                            }

                            // If lengths if (p1, p2) and (p1, p3) are same, then
                            // following conditions must met to form a square.
                            // 1) Square of length of (p1, p4) is same as twice
                            // the square of (p1, p2)
                            // 2) Square of length of (p2, p3) is same
                            // as twice the square of (p2, p4)
                            if (distB == distC && 2 * distB == distD
                                && 2 * DistSq(input[j], input[m]) == DistSq(input[j], input[k]))
                            {
                                Square square = new Square(input[i], input[j], input[k], input[m]);
                                squares.Add(square);
                                continue;
                            }
                            // The below two cases are similar to above case
                            if (distC == distD && 2 * distC == distB
                                && 2 * DistSq(input[k], input[j]) == DistSq(input[k], input[m]))
                            {
                                Square square = new Square(input[i], input[j], input[k], input[m]);
                                squares.Add(square);
                                continue;
                            }

                            if (distB == distD && 2 * distB == distC
                                && 2 * DistSq(input[j], input[k]) == DistSq(input[j], input[m]))
                            {
                                Square square = new Square(input[i], input[j], input[k], input[m]);
                                squares.Add(square);
                                continue;
                            }
                        }
                    }
                }
            }
            return squares;
        }

        private int DistSq(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
    }
}
