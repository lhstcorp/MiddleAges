using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace MiddleAges.Controllers
{
    public class CountryController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public CountryController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            Land land = _context.Lands.FirstOrDefault(k => k.LandId == player.CurrentLand);
            Country country = _context.Countries.FirstOrDefault(k => k.CountryId.ToString() == land.CountryId.ToString());

            if (country?.Name == "Independent lands")
            {
                return View("FoundCountry", player);
            }
            else
            {
                return View("Country", player);
            }     
            
            
        }

        [HttpPost]
        public async Task<IActionResult> FoundState(string playerId, string countryname, string countrycolor)
        {
            Player player = _context.Players.FirstOrDefault(k => k.Id == playerId);

            if (player?.Money >= 10000)
            {
                player.Money -= 10000;

                Country country = new Country();

                country.Name = countryname;
                country.RulerId = player.Id;
                country.Color = countrycolor;
                country.CapitalId = player.CurrentLand;

                if (country.Name == "")
                {
                    country.Name = player.CurrentLand + "state";
                }

                _context.Update(player);
                _context.Update(country);

                await _context.SaveChangesAsync();

                Land land = _context.Lands.FirstOrDefault(k => k.LandId == player.CurrentLand);
                land.CountryId = country.CountryId;

                _context.Update(land);

                await _context.SaveChangesAsync();
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Main"));
        }
    }
}
