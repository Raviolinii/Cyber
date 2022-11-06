using Cyber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cyber.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<AdminPanelController> _logger;
        public AdminPanelModel model { get; set; } = new AdminPanelModel();
        public AdminPanelController(UserManager<UserModel> userManager, ILogger<AdminPanelController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            model.UserList = _userManager.Users.ToList();
            return View(model);
        }
        public IActionResult CreateNewUser(string UserName, string Password)
        {
            UserModel user = new UserModel
            {
                UserName = UserName,
                Email = UserName,
                EmailConfirmed = true
            };
            IdentityResult result = _userManager.CreateAsync(user, Password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
                _logger.LogInformation($"User creation succeeded. Creator: {model.UserName}. Created: {user.UserName}");
            }
            else
                _logger.LogWarning($"User creation failed. Creator: {model.UserName}. Not created: {user.UserName}");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> BlockUser(string BlockedUserId)
        {
            var userToBlock = _userManager.FindByIdAsync(BlockedUserId).Result;

            DateTime lockoutEndDate = new DateTime(2999, 01, 01);
            await _userManager.SetLockoutEnabledAsync(userToBlock, true);
            await _userManager.SetLockoutEndDateAsync(userToBlock, lockoutEndDate);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(string DeletedUserId)
        {
            var userToDelete = _userManager.FindByIdAsync(DeletedUserId).Result;
            string userName = userToDelete.UserName;

            IdentityResult result = await _userManager.DeleteAsync(userToDelete);
            if (result.Succeeded)
                _logger.LogInformation($"User: {userName} deleted successfully by: {model.UserName}");
            else
                _logger.LogWarning($"User {userName} deletion failed by: {model.UserName}");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChangeUserPassword(string UserToChangePassId)
        {
            var userToChangePassword = _userManager.FindByIdAsync(UserToChangePassId).Result;
            var a = userToChangePassword.NormalizedUserName.Length;
            Random rand = new Random();
            int x = rand.Next(0, 100);
            var equation = a * Math.Sin(x);
            var newPassword = equation.ToString();
            userToChangePassword.PasswordHash = _userManager.PasswordHasher.HashPassword(userToChangePassword, newPassword);
            await _userManager.UpdateAsync(userToChangePassword);
            return RedirectToAction("Index");
        }


        public IActionResult EnableVerification()
        {
            PasswordVerification.PasswordVerificationEnabled = true;
            return RedirectToAction("Index");
        }
        public IActionResult DisableVerification()
        {
            PasswordVerification.PasswordVerificationEnabled = false;
            return RedirectToAction("Index");
        }
    }
}
