using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewComponents
{
    public class PlayerOverviewNavViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public PlayerOverviewNavViewComponent(ApplicationDbContext context,
                                              UserManager<Player> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;
            PlayerAttribute playerAttribute = await _context.PlayerAttributes.FirstOrDefaultAsync(pa => pa.PlayerId == player.Id);

            PlayerOverviewNavViewModel playerOverviewNavViewModel = new PlayerOverviewNavViewModel
            {
                Player = player,
                PlayerAttribute = playerAttribute
            };

            return View("PlayerOverviewNav", playerOverviewNavViewModel);
        }
    }
}
