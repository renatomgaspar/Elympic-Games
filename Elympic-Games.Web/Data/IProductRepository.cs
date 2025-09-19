using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        bool ProductExists(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(Product product);
        Task<bool> SaveAllAsync();
    }
}
