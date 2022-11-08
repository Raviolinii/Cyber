using Cyber.Models;
using Microsoft.AspNetCore.Identity;

namespace Cyber.Services
{
    public class ApplicationDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        public ApplicationDbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();          
            EnsureBothCreated(userManager);
        }
        public void EnsureBothCreated(UserManager<UserModel> userManager)
        {
            if (userManager.FindByEmailAsync("Administrator@gmail.com").Result == null)
            {
                var result = AddUser("Administrator", "Administrator@gmail.com", "P@$$w0rd", userManager).Result;
                if (result.Succeeded)
                    AddUserToRole("Administrator@gmail.com", "Administrator", userManager).Wait();
            }
            if (userManager.FindByEmailAsync("User@gmail.com").Result == null)
            {
                var result = AddUser("User", "User@gmail.com", "P@$$w0rd", userManager).Result;
                if (result.Succeeded)
                    AddUserToRole("User@gmail.com", "User", userManager).Wait();
            }
        }
        public async Task<IdentityResult> AddUser(string name, string email, string password, UserManager<UserModel> userManager)
        {
            UserModel user = new UserModel
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true
            };
            return await userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> AddUserToRole(string email, string role, UserManager<UserModel> userManager)
        {
            var user = userManager.FindByEmailAsync(email).Result;
            if (user is not null)
            {
                return await userManager.AddToRoleAsync(user, role);
            }
            else
                throw new Exception("User with provided email was not found");
        }

        public async Task<IdentityResult> DeleteUser(string email, UserManager<UserModel> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);
            return await userManager.DeleteAsync(user);
        }
    }
}
