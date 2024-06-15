using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Rental.Models;
using System.Security.Cryptography;
using System.Text;
using Rental.Services;
using System;

namespace Rental.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly AppContextdb _appContext;
        private readonly AccountService _accountService;

        public AuthorizationController(
            AppContextdb appContext,
            AccountService accountService)
        {
            _appContext = appContext;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (!ModelState.IsValid) return View(model);

            (bool, string) loginResult = _accountService.Login(model);

            if (!loginResult.Item1) return BadRequest("Неверный логин или пароль.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName!),
                new Claim(ClaimTypes.NameIdentifier, loginResult.Item2!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Exit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authorization");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User model)
        {
            if (!ModelState.IsValid)
                return View("Registration", model);

            var registrationResult = await _accountService.Register(model);

            if (!registrationResult)
            {
                ViewBag.ErrorMessage = "Пользователь с таким логином уже существует.";
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}