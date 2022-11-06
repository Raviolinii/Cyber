using Cyber.Models;
using Microsoft.AspNetCore.Identity;

namespace Cyber.Services
{
    public class ApplicationDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserAndRolesManager _userAndRolesManager;
        public ApplicationDbInitializer(IServiceProvider serviceProvider, IUserAndRolesManager userAndRolesManager)
        {
            _serviceProvider = serviceProvider;
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            _userAndRolesManager = userAndRolesManager;            
            EnsureBothCreated(userManager);
        }
        public void EnsureBothCreated(UserManager<UserModel> userManager)
        {
            if (userManager.FindByEmailAsync("Administrator@gmail.com").Result == null)
            {
                var result = _userAndRolesManager.AddUser("Administrator", "Administrator@gmail.com", "P@$$w0rd").Result;
                if (result.Succeeded)
                    _userAndRolesManager.AddUserToRole("Administrator@gmail.com", "Administrator").Wait();
            }
            if (userManager.FindByEmailAsync("User@gmail.com").Result == null)
            {
                var result = _userAndRolesManager.AddUser("User", "User@gmail.com", "P@$$w0rd").Result;
                if (result.Succeeded)
                    _userAndRolesManager.AddUserToRole("User@gmail.com", "User").Wait();
            }
        }
    }
}
