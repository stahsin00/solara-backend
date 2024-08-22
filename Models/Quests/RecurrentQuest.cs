namespace Solara.Models
{
    public class RecurrentQuest : Quest
    {
        public DateTime? EndDate { get; set; }
        public Repetition Repetition { get; set; }
    }
}
