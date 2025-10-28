namespace Elympic_Games.Web.Models.Events
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GameTypeName { get; set; }
    }
}
