using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewComponents
{
    public class ChatViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public ChatViewComponent(ApplicationDbContext context,
                                 UserManager<Player>  userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ChatMessage> chatMessages = await _context.ChatMessages.Include(p => p.Player).Where(m => m.ChatRoomType == (int) ChatRoomType.Global).OrderByDescending(m =>m.PublishingDateTime).Take(50).OrderBy(m => m.PublishingDateTime).ToListAsync();
            return View("Chat", chatMessages);
        }
    }
}
