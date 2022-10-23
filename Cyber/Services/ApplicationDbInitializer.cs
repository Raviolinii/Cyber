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
            this.SeedUser("Administrator",userManager);
            this.SeedUser("User",userManager);
        }
        public void SeedUser(string name, UserManager<UserModel> userManager)
        {
            if (userManager.FindByEmailAsync(name).Result == null)
            {
                UserModel user = new UserModel
                {
                    UserName = name,
                    Email = name + "@gmail.com",
                    EmailConfirmed = true
                };

                string password = "P@$$w0rd";
                IdentityResult result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, name).Wait();
                }
            }
        }
    }
}
