using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(
            DataContext context, 
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            var user = await _userHelper.GetUserByEmailAsync("elympicgames_manager@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Elympic",
                    LastName = "Games",
                    Email = "elympicgames_manager@gmail.com",
                    UserName = "elympicgames_manager@gmail.com",
                    PhoneNumber = "911111111"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            if (!_context.Products.Any())
            {
                AddProduct("PS5 Controller - Portugal", user);
                AddProduct("PS5 Controller - France", user);
                AddProduct("Logitech G Pro", user);
                AddProduct("Logitech MX Keys S", user);

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
