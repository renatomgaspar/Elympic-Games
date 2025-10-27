using Elympic_Games.Web.Models.News;

namespace Elympic_Games.Web.Helpers
{
    public interface IRssFeedHelper
    {
        Task<List<New>> GetEsportsNewsAsync(string rssUrl, int maxItems = 9);
    }
}
