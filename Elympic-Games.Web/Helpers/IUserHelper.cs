using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Elympic_Games.Web.Helpers
{
    public interface IUserHelper
    {
        Task<List<User>> GetAllAsync();

        Task<User> GetUserById(string id);

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password, bool isVerified);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> AddPasswordAsync(User user, string password);

        Task<IdentityResult> RemovePasswordAsync(User user);

        Task<IdentityResult> DeleteUserAsync(string id);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<bool> SendEmailToRecoryPassword(User user);

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<IEnumerable<SelectListItem>> GetUsersInRoleAsync(string role);

        IEnumerable<SelectListItem> GetComboRoles();
    }
}
