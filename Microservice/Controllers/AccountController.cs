﻿using IdentityServer.Models;
using IdentityServer4.Services;
using Microservice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    /// <summary>
    /// Defines endpoints to manage user account including registration
    /// and authentication
    /// </summary>
    public class AccountController : Controller
    {
        #region Fields

        private readonly IIdentityServerInteractionService _interactionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class
        /// </summary>
        /// <param name="interactionService">Identity Server interaction service</param>
        /// <param name="userManager">Asp.Net Identity user manager</param>
        /// <param name="signInManager">Asp.Net Identity sign-in manager</param>
        public AccountController(IIdentityServerInteractionService interactionService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl, Username = context?.LoginHint });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = model.ReturnUrl;
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var properties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                AllowRefresh = true,
                RedirectUri = model.ReturnUrl
            };

            await _signInManager.SignInAsync(user, properties);
            return Redirect(_interactionService.IsValidReturnUrl(model.ReturnUrl) ? model.ReturnUrl : "~/");
        }
    }
}
