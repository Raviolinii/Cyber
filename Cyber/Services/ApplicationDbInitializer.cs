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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            this.SeedUser("Administrator",userManager);
            this.SeedUser("User",userManager);
        }
        public void SeedUser(string name, UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync(name).Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = name,
                    Email = name,
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
