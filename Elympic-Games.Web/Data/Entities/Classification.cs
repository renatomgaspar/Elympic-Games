using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Classification : IEntity
    {
        public int Id { get; set; }


        [Required]
        public int Rank { get; set; }


        [Required]
        public int Points { get; set; }


        [Required]
        public int EventId { get; set; }

        public Event Event { get; set; }


        [Required]
        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}
