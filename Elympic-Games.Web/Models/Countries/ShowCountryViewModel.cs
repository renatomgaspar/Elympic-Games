namespace Elympic_Games.Web.Models.Countries
{
    public class ShowCountryViewModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryImage { get; set; }
        public List<string> Games { get; set; }
    }
}
