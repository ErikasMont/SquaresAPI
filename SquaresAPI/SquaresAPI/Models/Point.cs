using System.ComponentModel.DataAnnotations;

namespace SquaresAPI.Models
{
    public class Point
    {
        public int Id { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
    }
}
