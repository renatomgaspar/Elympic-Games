using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Elympic_Games.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password, bool isVerified)
        {
            MailMessage email = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            email.From = new MailAddress("schoolmanagerpws@gmail.com");
            email.To.Add(user.Email);

            email.Subject = "Account Activation";

            email.IsBodyHtml = true;
            email.Body = $"Click to make your account Active <a href='https://localhost:44387/Account/Activate/?id={user.Id}'>> HERE <</a>";

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("schoolmanagerpws@gmail.com", "lzqf lrqa jywi agkj");
            smtp.EnableSsl = true;
            smtp.Send(email);

            if (isVerified)
            {
                user.EmailConfirmed = true;
                return await _userManager.CreateAsync(user, password);
            }
            else
            {
                user.EmailConfirmed = false;
                return await _userManager.CreateAsync(user, password);
            }
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            if (!user.EmailConfirmed)
            {
                return SignInResult.NotAllowed;
            }

            return await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser.Email != user.Email)
            {
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.Email;
                }
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user,
            string oldPassword,
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<IEnumerable<SelectListItem>> GetUsersInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return users
                .Select(u => new SelectListItem
                {
                    Text =  u.Email,
                    Value = u.Id
                })
                .OrderBy(u => u.Text)
                .ToList();
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .OrderBy(r => r.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a role...",
                Value = string.Empty
            });

            return list;
        }
    }
}
