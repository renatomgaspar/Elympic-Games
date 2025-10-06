using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Products;

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
    }
}
