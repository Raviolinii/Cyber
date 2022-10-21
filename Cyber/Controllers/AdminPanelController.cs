using Cyber.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cyber.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminPanelModel model { get; set; }
        public AdminPanelController(UserManager<IdentityUser> userManager)
        {
           _userManager = userManager;
            model.UserList= _userManager.Users.ToList();
        }
        public IActionResult Index()
        {
            return View();
        }
        public void CreateNewUser(string name, string password)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = name,
                Email = name,
                EmailConfirmed = true
            };
            IdentityResult result = _userManager.CreateAsync(user, password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, name).Wait();
            }
        }
    }
}
