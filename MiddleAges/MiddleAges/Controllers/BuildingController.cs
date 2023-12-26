using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    public class BuildingController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BuildingController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
            
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Player player = _context.Players.FirstOrDefault(k => k.PlayerId.ToString() == user.Id);

            if (player.Money >= 100)
            {
                player.Money -= 100;
                building.Lvl += 1;

                _context.Update(building);
                _context.Update(player);

                await _context.SaveChangesAsync();
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}
