using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreSignalRWithAuth.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreSignalRWithAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FullName = model.FullName
                    };
                    var createResult = await _userManager.CreateAsync(user, model.Password);
                    if (createResult.Succeeded)
                    {
                        await _userManager.AddClaimsAsync(user, new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Id),
                            new Claim("sys:FullName",model.FullName)
                        });
                        var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (!loginResult.Succeeded)
                            return RedirectToAction("Login");
                        return RedirectToAction("Index", "Home");

                    }
                    foreach(string error in createResult.Errors.Select(c => c.Description))
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                ModelState.AddModelError("", "User already existed");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (loginResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Username or password is invalid");
                }
                else
                {
                    ModelState.AddModelError("", "User doesn't exist");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}