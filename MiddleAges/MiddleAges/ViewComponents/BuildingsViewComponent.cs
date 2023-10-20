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
    public class BuildingsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public BuildingsViewComponent(ApplicationDbContext context,
                                      UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            List<Building> buildings = _context.buildings.Where(k => k.PlayerId.ToString() == user.Id).ToList();
            return View("Buildings", buildings);
        }
    }
}
