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
        private readonly ILogger<ChatController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public ChatController(ILogger<ChatController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {            
            return View();
        }
    }
}
