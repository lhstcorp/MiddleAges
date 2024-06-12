using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
        public async Task<IActionResult> Recruit(string unitId, int count)
        {
            Unit unit = _context.Units.FirstOrDefault(k => k.UnitId.ToString() == unitId);
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            //Building building = _context.Buildings.FirstOrDefault(k => k.BuildingId.ToString() == buildingId);
            //Building building = _context.Buildings.FirstOrDefault(k => k.PlayerId.ToString() == player.Id);
            long requiredMoney = unit.Type switch
            {
                (int)UnitType.Peasant => count * (int)UnitPrice.Peasant,
                (int)UnitType.Soldier => count * (int)UnitPrice.Soldier,
                _ => 0
            };
            if (player.Money >= requiredMoney
             && requiredMoney > 0 && player.RecruitAmount >= count)
            {
                player.Money -= requiredMoney;
                unit.Count += count;
                player.RecruitAmount -= count;
                _context.Update(unit);
                _context.Update(player);
                await _context.SaveChangesAsync();
            }
           
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
        [HttpPost]
        public async Task<IActionResult> Dismiss(string unitId, int dismiss)
        {
            Unit unit = _context.Units.FirstOrDefault(k => k.UnitId.ToString() == unitId);
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            //Building building = _context.Buildings.FirstOrDefault(k => k.BuildingId.ToString() == buildingId);
            //Building building = _context.Buildings.FirstOrDefault(k => k.PlayerId.ToString() == player.Id);
            double returnMoney = unit.Type switch
            {
                (int)UnitType.Peasant => dismiss * (double)UnitPrice.Peasant/2.00,
                (int)UnitType.Soldier => dismiss * (double)UnitPrice.Soldier/2.00,
                _ => 0
            };
            if (unit.Count > 0 && unit.Count >= dismiss)
            {
                player.Money += returnMoney;
                unit.Count -= dismiss;
                player.RecruitAmount += dismiss;
                _context.Update(unit);
                _context.Update(player);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}