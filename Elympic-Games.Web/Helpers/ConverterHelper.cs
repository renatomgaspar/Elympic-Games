using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Elympic_Games.Web.Models.Arenas;
using Elympic_Games.Web.Models.Cities;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Events;
using Elympic_Games.Web.Models.Gametypes;
using Elympic_Games.Web.Models.Players;
using Elympic_Games.Web.Models.Products;
using Elympic_Games.Web.Models.Teams;

namespace Elympic_Games.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel model, Guid imageId, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id,
                IsAvailable = model.IsAvailable,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                ImageId = imageId,
                User = model.User
            };
        }

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ImageId = product.ImageId,
                User = product.User
            };
        }

        public User ToUser(RegisterNewUserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId
            };
        }

        public RegisterNewUserViewModel ToRegisterNewUserViewModel(User user)
        {
            return new RegisterNewUserViewModel
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageFile = null
            };
        }

        public Country ToCountry(CountryViewModel model, Guid imageId, bool isNew)
        {
            return new Country
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Code = model.Code,
                ImageId = imageId
            };
        }

        public CountryViewModel ToCountryViewModel(Country country)
        {
            return new CountryViewModel
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code,
                ImageId = country.ImageId,
                ImageFile = null
            };
        }

        public GameType ToGametype(GametypeViewModel model, Guid imageId, bool isNew)
        {
            return new GameType
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                TeamSize = model.TeamSize,
                ActivePlayerNo = model.ActivePlayerNo,
                ImageId = imageId
            };
        }

        public GametypeViewModel ToGametypeViewModel(GameType gametype)
        {
            return new GametypeViewModel
            {
                Id = gametype.Id,
                Name = gametype.Name,
                TeamSize = gametype.TeamSize,
                ActivePlayerNo = gametype.ActivePlayerNo,
                ImageId = gametype.ImageId,
                ImageFile = null
            };
        }

        public Team ToTeam(TeamViewModel model, bool isNew)
        {
            return new Team
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                CountryId = model.CountryId,
                GameTypeId = model.GameTypeId,
                TeamManagerId = model.TeamManagerId
            };
        }

        public TeamViewModel ToTeamViewModel(Team team)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                CountryId = team.CountryId,
                GameTypeId = team.GameTypeId,
                TeamManagerId = team.TeamManagerId
            };
        }

        public Player ToPlayer(PlayerViewModel model, bool isNew)
        {
            return new Player
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                TeamId = model.TeamId,
                IsPlaying = model.IsPlaying
            };
        }

        public PlayerViewModel ToPlayerViewModel(Player player)
        {
            return new PlayerViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                TeamId = player.TeamId,
                IsPlaying = player.IsPlaying
            };
        }

        public City ToCity(CityViewModel model, bool isNew)
        {
            return new City
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                CountryId = model.CountryId
            };
        }

        public CityViewModel ToCityViewModel(City city)
        {
            return new CityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = city.CountryId
            };
        }

        public Arena ToArena(ArenaViewModel model, bool isNew)
        {
            return new Arena
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                CityId = model.CityId,
                TotalCapacity = model.TotalCapacity,
                AccessibleSeating = model.AccessibleSeating
            };
        }

        public ArenaViewModel ToArenaViewModel(Arena arena)
        {
            return new ArenaViewModel
            {
                Id = arena.Id,
                Name = arena.Name,
                CityId = arena.CityId,
                TotalCapacity = arena.TotalCapacity,
                AccessibleSeating = arena.AccessibleSeating
            };
        }

        public Event ToEvent(EventViewModel model, bool isNew)
        {
            return new Event
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ArenaId = model.ArenaId
            };
        }

        public EventViewModel ToEventViewModel(Event eventObj)
        {
            return new EventViewModel
            {
                Id = eventObj.Id,
                Name = eventObj.Name,
                StartDate = eventObj.StartDate,
                EndDate = eventObj.EndDate,
                ArenaId = eventObj.ArenaId
            };
        }
    }
}
