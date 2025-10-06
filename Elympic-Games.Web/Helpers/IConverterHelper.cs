using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Products;

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
    }
}
