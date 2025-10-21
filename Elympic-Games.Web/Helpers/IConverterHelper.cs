using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Elympic_Games.Web.Models.Arenas;
using Elympic_Games.Web.Models.Cities;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Events;
using Elympic_Games.Web.Models.Gametypes;
using Elympic_Games.Web.Models.Matches;
using Elympic_Games.Web.Models.Players;
using Elympic_Games.Web.Models.Products;
using Elympic_Games.Web.Models.Teams;
using Elympic_Games.Web.Models.Tickets;

namespace Elympic_Games.Web.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, Guid imageId, bool isNew);

        ProductViewModel ToProductViewModel(Product product);

        User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew);

        RegisterNewUserViewModel ToRegisterNewUserViewModel(User user);

        Country ToCountry(CountryViewModel model, Guid imageId, bool isNew);

        CountryViewModel ToCountryViewModel(Country country);

        GameType ToGametype(GametypeViewModel model, Guid imageId, bool isNew);

        GametypeViewModel ToGametypeViewModel(GameType gametype);

        Team ToTeam(TeamViewModel model, bool isNew);

        TeamViewModel ToTeamViewModel(Team team);

        Player ToPlayer(PlayerViewModel model, bool isNew);

        PlayerViewModel ToPlayerViewModel(Player player);

        City ToCity(CityViewModel model, bool isNew);

        CityViewModel ToCityViewModel(City city);

        Arena ToArena(ArenaViewModel model, bool isNew);

        ArenaViewModel ToArenaViewModel(Arena arena);

        Event ToEvent(EventViewModel model, bool isNew);

        EventViewModel ToEventViewModel(Event eventObj);

        Match ToMatch(MatchViewModel model, bool isNew);

        MatchViewModel ToMatchViewModel(Match match);

        InsertScoresViewModel ToInsertScoresViewModel(Match match);

        Ticket ToTicket(TicketsViewModel model, bool isNew);

        TicketsViewModel ToTicketViewModel(Ticket ticket);
    }
}
