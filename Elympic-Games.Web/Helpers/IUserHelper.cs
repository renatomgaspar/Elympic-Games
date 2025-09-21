using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Elympic_Games.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
