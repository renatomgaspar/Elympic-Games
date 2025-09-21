using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();
    }
}
