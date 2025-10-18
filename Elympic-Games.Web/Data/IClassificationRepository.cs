using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Data
{
    public interface IClassificationRepository : IGenericRepository<Classification>
    {
        Task<List<Classification>> GetClassificationsByEventIdAsync(int eventId);

        Task CreateClassification(Match match);
    }
}
