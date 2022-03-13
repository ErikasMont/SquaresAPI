namespace SquaresAPI.Models
{
    public class Square
    {
        public Point pointA { get; set; }
        public Point pointB { get; set; }
        public Point pointC { get; set; }
        public Point pointD { get; set; }

        public Square(Point a, Point b, Point c, Point d)
        {
            pointA = a;
            pointB = b;
            pointC = c;
            pointD = d;
        }
    }
}
