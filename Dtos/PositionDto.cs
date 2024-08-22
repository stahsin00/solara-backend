using System.ComponentModel.DataAnnotations;

namespace Solara.Dtos
{
    public class PositionDto
    {
        [Range(1, 4, ErrorMessage = "Position must be between 1 and 4.")]
        public int Position { get; set; }
    }
}