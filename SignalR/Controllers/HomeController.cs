using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.DAL;
using SignalR.Hubs;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHubContext<ChatHub> _hubContext;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Chat()
        {

            List<AppUser> users = _userManager.Users.Where(u => u.ConnectionId != null).ToList();
            ViewBag.ChatUsers = users;
            return View();
        }

        public async Task<IActionResult> CreateUser()
        {
            var result1 = await _userManager.CreateAsync(new AppUser { UserName = "_samir", Fullname = "Samir" }, "12345@Li");
            var result2 = await _userManager.CreateAsync(new AppUser { UserName = "_orxan", Fullname = "Orxan" }, "12345@Li");
            var result3 = await _userManager.CreateAsync(new AppUser { UserName = "_reshid", Fullname = "Reshid" }, "12345@Li");
            var result4 = await _userManager.CreateAsync(new AppUser { UserName = "_ferid", Fullname = "Ferid" }, "12345@Li");
            return Ok("created");

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user = _userManager.FindByNameAsync(model.Username).Result;
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            return RedirectToAction("Index");

        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PrivateSend(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            await _hubContext.Clients.Client(appUser.ConnectionId).SendAsync("PrivateMessage");
            return RedirectToAction("chat");
        }
    }
}
