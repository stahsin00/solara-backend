using System.ComponentModel.DataAnnotations;

namespace Solara.Dtos
{
    public class GameDto
    {
        public int Id { get; set;}

        [Range(1, int.MaxValue, ErrorMessage = "Minutes must be greater than 0.")]
        public int Minutes { get; set;}
    }
}