using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Enums;
using System.Drawing;

namespace MiddleAges.Controllers
{
    public class BuildingController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public BuildingController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
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
        public async Task<IActionResult> LvlUp(string buildingId)
        {
            Building building = _context.Buildings.FirstOrDefault(k => k.BuildingId.ToString() == buildingId);

            var player = await _userManager.GetUserAsync(HttpContext.User);

            long requiredMoney = building.Type switch
            {
                (int)BuildingType.Estate => (int)BuildingPrice.Estate ,
                (int)BuildingType.Barracks => (int)BuildingPrice.Barracks ,
                _ => 0
            };

            if (player.Money >= requiredMoney
             && requiredMoney > 0)
            {
                player.Money -= requiredMoney;
                building.Lvl += 1;

                _context.Update(building);
                _context.Update(player);

                await _context.SaveChangesAsync();
            }


            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }

    }
}
