// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Cyber.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cyber.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserManager<UserModel> _userManager;

        public LogoutModel(SignInManager<UserModel> signInManager, ILogger<LogoutModel> logger, UserManager<UserModel> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User: {user.UserName} logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
