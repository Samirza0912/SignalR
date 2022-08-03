using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalR.Models;
using SignalR.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            var result = _userManager.CreateAsync(new AppUser { UserName = "Samir", Fullname = "Samir" }, "12345@Sa").Result;
            var result2 = _userManager.CreateAsync(new AppUser { UserName = "Orxan", Fullname = "Orxan" }, "12345@Sa").Result;
            var result3 = _userManager.CreateAsync(new AppUser { UserName = "Reshid", Fullname = "Reshid" }, "12345@Sa").Result;
            var result4 = _userManager.CreateAsync(new AppUser { UserName = "Ferid", Fullname = "Ferid" }, "12345@Sa").Result;
            return Ok("Users created");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            var user = _userManager.FindByNameAsync(login.Username).Result;
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, true);
            return RedirectToAction("chat");
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("index");
        }
    }
}
