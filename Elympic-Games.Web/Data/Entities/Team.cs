using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Team : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }


        [Required]
        public int GameTypeId { get; set; }
        public GameType GameType { get; set; }


        [Required]
        public string TeamManagerId { get; set; }

        public User TeamManager { get; set; }
    }
}
