using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewComponents
{
    public class UnitsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public UnitsViewComponent(ApplicationDbContext context,
                                  UserManager<Player> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            List<Unit> units = _context.Units.Where(k => k.PlayerId == player.Id).ToList();
            List<Building> buildings = _context.Buildings.Where(b => b.PlayerId == player.Id).ToList();

            UnitsBuildingsViewModel unitsBuildingsViewModel = new UnitsBuildingsViewModel
            {
                Player = player,
                Units = units,
                Buildings = buildings
            };

            return View("Units", unitsBuildingsViewModel);
        }
    }
}
