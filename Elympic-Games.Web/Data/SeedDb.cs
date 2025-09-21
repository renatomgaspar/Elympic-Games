using Elympic_Games.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!_context.Products.Any())
            {
                AddProduct("Portugal - PS5 Controller");
                AddProduct("France - PS5 Controller");
                AddProduct("Logitech G Pro");
                AddProduct("Logitech MX Keys S");

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100)
            });
        }
    }
}
