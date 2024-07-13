using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MiddleAges.Controllers;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Threading.Tasks;

namespace MiddleAges.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public ChatHub(ILogger<ChatHub> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessageToGlobalChat(string message)
        {            
            Player player = await _userManager.GetUserAsync(Context.User);
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.PlayerId = player.Id;
            chatMessage.MessageValue = message;
            chatMessage.ChatRoomType = (int) ChatRoomType.Global;

            await _context.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync(
                "ReceiveMessageFromGlobalChat",
                player.Id,
                player.UserName, 
                player.ImageURL, 
                message,
                chatMessage.PublishingDateTime);
            
            _logger.LogInformation("Message {Message} was sent in global chat", message);
                
        }
    }
}
