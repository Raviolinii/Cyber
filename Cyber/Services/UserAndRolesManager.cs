using Cyber.Models;
using Microsoft.AspNetCore.Identity;

namespace Cyber.Services
{
    public class UserAndRolesManager : IUserAndRolesManager
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<UserAndRolesManager> _logger;

        public UserAndRolesManager(UserManager<UserModel> userManager, ILogger<UserAndRolesManager> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> AddUser(string name, string email, string password)
        {
            UserModel user = new UserModel
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true
            };
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> AddUserToRole(string email, string role)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user is not null)
            {
                return await _userManager.AddToRoleAsync(user, role);
            }
            else
                throw new Exception("User with provided email was not found");
        }

        public async Task<IdentityResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await _userManager.DeleteAsync(user);
        }
    }
}
