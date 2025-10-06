using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Data
{
    public interface IGametypeRepository : IGenericRepository<GameType>
    {
        Task<GameType?> GametypeExistsByName(string name);
    }
}
