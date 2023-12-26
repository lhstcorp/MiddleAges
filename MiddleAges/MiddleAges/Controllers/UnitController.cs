using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    public class UnitController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UnitController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        public async Task<IActionResult> Recruit(string unitId, int count)
        {
            Unit unit = _context.Units.FirstOrDefault(k => k.UnitId.ToString() == unitId);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            Player player = _context.Players.FirstOrDefault(k => k.PlayerId.ToString() == user.Id);

            long requiredMoney = unit.Type switch
            {
                (int)UnitType.Peasant => count * (int)UnitPrice.Peasant,
                (int)UnitType.Soldier => count * (int)UnitPrice.Soldier,
                _ => 0
            };

            if (player.Money >= requiredMoney
             && requiredMoney > 0)
            {
                player.Money -= requiredMoney;
                unit.Count += count;

                _context.Update(unit);
                _context.Update(player);

                await _context.SaveChangesAsync();
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}