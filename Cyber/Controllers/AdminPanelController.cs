﻿using Cyber.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cyber.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AdminPanelModel model { get; set; } = new AdminPanelModel();
        public AdminPanelController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            model.UserList = _userManager.Users.ToList();
            return View(model);
        }
        public IActionResult CreateNewUser(string UserName, string Password)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = UserName,
                Email = UserName,
                EmailConfirmed = true
            };
            IdentityResult result = _userManager.CreateAsync(user, Password).Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
            }
            return RedirectToAction("Index");
        }
        public IActionResult BlockUser(string BlockedUserId)
        {
            var userToBlock = _userManager.FindByIdAsync(BlockedUserId).Result;
            _userManager.SetLockoutEnabledAsync(userToBlock, true);
            return RedirectToAction("Index");
        }
        //public void UserListUpdate()
        //{
        //    model.
        //}
    }
}
