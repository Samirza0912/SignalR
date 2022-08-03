using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR.DAL;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class ChatHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public ChatHub(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("dd.MM.yyyy"));
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
                _context.SaveChanges();
            }

            return base.OnConnectedAsync();
        }
    }
}
