using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MiddleAges.ViewModels;
using MiddleAges.Enums;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public MainController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            player = await _context.Players.Include(p => p.Land).ThenInclude(l => l.Country).FirstOrDefaultAsync(p => p.Id == player.Id);

            Land residenceLand = await _context.Lands.Include(l => l.Country).FirstOrDefaultAsync(l => l.LandId == player.ResidenceLand);

            List<Unit> units = await _context.Units.Where(u => u.PlayerId == player.Id).ToListAsync();

            PlayerAttribute playerAttribute = await _context.PlayerAttributes.FirstOrDefaultAsync(pa => pa.PlayerId == player.Id);

            MainInfoViewModel mainInfoViewModel = new MainInfoViewModel
            {
                Player = player,
                PlayerAttribute = playerAttribute,
                ResidenceLand = residenceLand,
                Units = units
            };
            return View("Main", mainInfoViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> RestartProduction()
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);

            if (player != null)
            {
                player.EndDateTimeProduction = DateTime.UtcNow.AddDays(1);
                _context.Update(player);

                await _context.SaveChangesAsync();

                result = "Ok";
            }

            return Json(JsonSerializer.Serialize(result));
        }

        public async Task<IActionResult> UpgradeAttribute(string attributename)
        {
            string result = "Error";

            Player player = await _userManager.GetUserAsync(HttpContext.User);
            PlayerAttribute playerAttribute = await _context.PlayerAttributes.FirstOrDefaultAsync(pa => pa.PlayerId == player.Id);
            int availAttrPoints = CommonLogic.GetAvailAttrPoints(player.Lvl, playerAttribute);

            if (player != null
             && availAttrPoints > 0
             && attributename != null)
            {
                switch (attributename)
                {
                    case "Management":
                        playerAttribute.Management += 1;
                        result = playerAttribute.Management.ToString();
                        break;
                    case "Warfare":
                        playerAttribute.Warfare += 1;
                        result = playerAttribute.Warfare.ToString();
                        break;
                    case "Leadership":
                        playerAttribute.Leadership += 1;
                        result = playerAttribute.Leadership.ToString();
                        break;
                }

                _context.Update(playerAttribute);

                await _context.SaveChangesAsync();
            }

            return Json(JsonSerializer.Serialize(result));
        }

        public JsonResult GetPlayerById(string id)
        {
            var playerQuery = _context.Players
                                .Where(p => p.Id == id)
                                .Join(_context.Ratings,
                                        p => p.Id,
                                        r => r.PlayerId,
                                        (p, r) => new { Player = p, Rating = r })
                                .Join(_context.PlayerInformations,
                                        combined => combined.Player.Id,
                                        pi => pi.PlayerId,
                                        (combined, pi) => new { combined.Player, combined.Rating, PlayerInformation = pi })
                                .Join(_context.Units,
                                        combined => combined.Player.Id,
                                        u => u.PlayerId,
                                        (combined, u) => new { combined.Player, combined.Rating, combined.PlayerInformation, Unit = u })
                                .Where(combined => combined.Unit.Type == (int)UnitType.Peasant)
                                .Join(_context.Lands,
                                        combined => combined.Player.ResidenceLand,
                                        l => l.LandId,
                                        (combined, l) => new { combined.Player, combined.Rating, combined.PlayerInformation, combined.Unit, ResidenceLand = l })
                                .Join(_context.Countries,
                                        combined => combined.ResidenceLand.CountryId,
                                        l => l.CountryId,
                                        (combined, c) => new { combined.Player, combined.Rating, combined.PlayerInformation, combined.Unit, combined.ResidenceLand, ResidenceCountry = c })
                                //.Join(_context.Lands,
                                //        combined => combined.Player.CurrentLand,
                                //        l => l.LandId,
                                //        (combined, l) => new { combined.Player, combined.Rating, combined.PlayerInformation, combined.Unit, combined.ResidenceLand, combined.ResidenceCountry, CurrentLand = l })
                                //.Join(_context.Countries,
                                //        combined => combined.CurrentLand.CountryId,
                                //        l => l.CountryId,
                                //        (combined, c) => new { combined.Player, combined.Rating, combined.PlayerInformation, combined.Unit, combined.ResidenceLand, combined.ResidenceCountry, combined.CurrentLand, CurrentCountry = c })
                                //.Select(combined => new { Player = combined.Player, Rating = combined.Rating, PlayerInformation = combined.PlayerInformation, Unit = combined.Unit, ResidenceLand = combined.ResidenceLand, ResidenceCountry = combined.ResidenceCountry, CurrentLand = combined.CurrentLand, CurrentCountry = combined.CurrentCountry }).FirstOrDefault();
                                .Select(combined => new { Player = combined.Player, Rating = combined.Rating, PlayerInformation = combined.PlayerInformation, Unit = combined.Unit, ResidenceLand = combined.ResidenceLand, ResidenceCountry = combined.ResidenceCountry }).FirstOrDefault();

            ModalPlayerViewModel modalPlayerViewModel = new ModalPlayerViewModel
            {
                Player = playerQuery.Player,
                Rating = playerQuery.Rating,
                PlayerInformation = playerQuery.PlayerInformation,                
                ResidenceLand = playerQuery.ResidenceLand,
                ResidenceCountry = playerQuery.ResidenceCountry,
                Peasants = playerQuery.Unit
            };

            return Json(JsonSerializer.Serialize(modalPlayerViewModel));
        }
    }
}
