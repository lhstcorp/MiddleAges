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
        private readonly UserManager<Player> _userManager;

        public UnitController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
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
        public async Task<IActionResult> Recruit(string unitId, int count, string buildingId)
        {
            Unit unit = _context.Units.FirstOrDefault(k => k.UnitId.ToString() == unitId);

            Player player = await _userManager.GetUserAsync(HttpContext.User);

            //Building building = _context.Buildings.FirstOrDefault(k => k.BuildingId.ToString() == buildingId);
            Building building = _context.Buildings.FirstOrDefault(k => k.PlayerId.ToString() == player.Id);


            long requiredMoney = unit.Type switch
            {
                (int)UnitType.Peasant => count * (int)UnitPrice.Peasant,
                (int)UnitType.Soldier => count * (int)UnitPrice.Soldier,
                _ => 0
            };

            if (player.Money >= requiredMoney
             && requiredMoney > 0 && (building.Lvl*10) >= count)
            {
                player.Money -= requiredMoney;
                unit.Count += count;

                _context.Update(unit);
                _context.Update(player);

                await _context.SaveChangesAsync();
            }

            //else
            //{
            //    Exception exception = new Exception("Fucking fuck");
            //}

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}