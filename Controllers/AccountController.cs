﻿using Microsoft.AspNetCore.Mvc;
using PokedexWebApp.Repositories;
using PokedexWebApp.ViewModels;

namespace PokedexWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IAccountRepository _repo { get; }

        public AccountController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userViewModel)
        {
            userViewModel.UserName = userViewModel.Email; // Assign Email to UserName
            ModelState.Remove("UserName");
            if (ModelState.IsValid)
            {
                var result = await _repo.SignUpUserAsync(userViewModel);
                if (result)
                {
                    TempData["SuccessNotification"] = "Registration successful. You can now login."; // Add success notification to TempData
                    return RedirectToAction("Login");
                }
            }
            return View(userViewModel);
        }




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // login activity -> cookie [Roles and Claims]
                var result = await _repo.SignInUserAsync(userViewModel);
                //login cookie and transfter to the client 
                if (result is not null)
                {
                    // add token to session 
                    HttpContext.Session.SetString("JWToken", result);
                    // Add success notification to TempData
                    TempData["SuccessNotification"] = "Login successful.";

                    return RedirectToAction("GetAllPokemon", "Pokemon");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Credentials");
            }
            return View(userViewModel);
        }

        public IActionResult Logout()
        {
            // Clear the session or any other authentication-related data
            HttpContext.Session.Clear();

            // Redirect to the login page or any other desired page
            return RedirectToAction("Login", "Account");
        }

        private bool IsUserAuthenticated()
        {
            // Check if the token exists in the session

            return HttpContext.Session.GetString("JWToken") != null;
        }

    }
}