namespace Solara.Dtos
{
    public class QuestDto
    {
        public string Name { get; set; } = "Unnamed Quest";
        public string Description { get; set; } = "No Description";

        public DateTime? Deadline { get; set; }

        public string Difficulty { get; set; } = "Unspecified";  // TODO
        public bool Important { get; set; } = false;

        public DateTime? EndDate { get; set; }
        public string? Repetition { get; set; }
    }
}