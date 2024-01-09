using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;
namespace MiddleAges.Controllers
{
    public class CountryController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CountryController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            Player player = _context.Players.FirstOrDefault(k => k.PlayerId.ToString() == user.Id);
            Land land = _context.Lands.FirstOrDefault(k => k.LandId == player.CurrentLand);
            Country country = _context.Countries.FirstOrDefault(k => k.CountryId.ToString() == land.CountryId.ToString());

            if (country.Name == "Independent lands")
            {
                return View("FoundCountry", player);
            }
            else
            {
                return View("Country", player);
            }     
            
            
        }
    }
}
