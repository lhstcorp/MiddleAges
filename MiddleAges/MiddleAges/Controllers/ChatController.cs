using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiddleAges.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public ChatController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChatMessage(string messageValue)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);

            ChatMessage chatMessage = new ChatMessage();

            chatMessage.PlayerId = player.Id;
            chatMessage.MessageValue = messageValue;
            chatMessage.ChatRoomType = (int)ChatRoomType.General;
            chatMessage.PublishingDateTime = DateTime.UtcNow;
            _context.Update(chatMessage);

            await _context.SaveChangesAsync();

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}
