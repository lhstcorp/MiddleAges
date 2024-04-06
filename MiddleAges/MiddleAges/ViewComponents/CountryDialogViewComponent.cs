using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiddleAges.Data;
using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewComponents
{
    public class CountryDialogViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public CountryDialogViewComponent(ApplicationDbContext context,
                                      UserManager<Player> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            List<Building> buildings = _context.Buildings.Where(k => k.PlayerId == player.Id).ToList();
            return View("Buildings", buildings);
        }
    }
}
