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

            //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            //var ipAddress = remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
            //    ? remoteIpAddress.MapToIPv4().ToString()
            //    : remoteIpAddress.ToString();

            CommonLogic.UpdateLastPlayerActivityDateTime(_context, player);

            player = await _context.Players.Include(p => p.Land).ThenInclude(l => l.Country).FirstOrDefaultAsync(p => p.Id == player.Id);

            Land residenceLand = await _context.Lands.Include(l => l.Country).FirstOrDefaultAsync(l => l.LandId == player.ResidenceLand);

            List<Unit> units = await _context.Units.Where(u => u.PlayerId == player.Id).ToListAsync();

            PlayerAttribute playerAttribute = await _context.PlayerAttributes.FirstOrDefaultAsync(pa => pa.PlayerId == player.Id);
            List<PlayerLocalEvent> playerLocalEvents = await _context.PlayerLocalEvents.Where(le => le.PlayerId == player.Id).ToListAsync();

            LandDevelopmentShare landDevelopmentShare = await _context.LandDevelopmentShares.FirstOrDefaultAsync(ld => ld.LandId == player.ResidenceLand);

            double peasantHourIncome = CommonLogic.BasePeasantIncome * CommonLogic.LandsCount * landDevelopmentShare.MarketShare;

            List<Player> onlinePlayers = await _context.Players.OrderByDescending(p => p.LastActivityDateTime).Where(p => p.LastActivityDateTime > DateTime.UtcNow.AddMinutes(-CommonLogic.PlayerOnlineMinutes)).ToListAsync();

            var userAgent = Request.Headers["User-Agent"].ToString();
            var deviceType = userAgent.Contains("Mobi") ? "Mobile" : "Desktop";

            MainInfoViewModel mainInfoViewModel = new MainInfoViewModel
            {
                Player = player,
                PlayerAttribute = playerAttribute,
                ResidenceLand = residenceLand,
                Units = units,
                PlayerLocalEvents = playerLocalEvents,
                PeasantHourIncome = peasantHourIncome,
                OnlinePlayers = onlinePlayers,
                DeviceType = deviceType
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
                Peasants = playerQuery.Unit,
                PlayerDescription = GeneratePlayerDescription(id, playerQuery.ResidenceCountry.Name, playerQuery.ResidenceLand.LandId),
                LvlProgressBarValue = playerQuery.Player.Exp - CommonLogic.GetRequiredExpByLvl(playerQuery.Player.Lvl),
                LvlProgressBarMaxValue = CommonLogic.GetRequiredExpByLvl(playerQuery.Player.Lvl + 1) - CommonLogic.GetRequiredExpByLvl(playerQuery.Player.Lvl),
                NextLvlRequiredExp = CommonLogic.GetRequiredExpByLvl(playerQuery.Player.Lvl + 1)
            };

            return Json(JsonSerializer.Serialize(modalPlayerViewModel));
        }

        private string GeneratePlayerDescription(string playerId, string residenceCountryName, string residenceLandId)
        {
            string playerDescription = "";

            List<Country> playerCountriesAsKing = _context.Countries.Where(c => c.RulerId == playerId).ToList();
                      
            if (playerCountriesAsKing.Count > 0)
            {
                playerDescription = "";

                foreach (Country c in playerCountriesAsKing)
                {
                    if (playerDescription.Length > 0)
                    {
                        playerDescription += ", ";
                    }

                    playerDescription += c.Name;
                }

                playerDescription = "The ruler of " + playerDescription;
            }

            playerDescription += ". Citizen of " + residenceCountryName + " (" + residenceLandId + ")";

            return playerDescription;
        }

        public JsonResult GetLandById(string id)
        {
            var landQuery = _context.Lands
                                .Where(l => l.LandId == id)
                                .Join(_context.Countries,
                                    l => l.CountryId,
                                    c => c.CountryId,
                                    (l, c) => new { Land = l, Country = c })
                                .Join(_context.LandBuildings,
                                    combined => combined.Land.LandId,
                                    lb => lb.LandId,
                                    (combined, lb) => new { combined.Land, combined.Country, LandBuilding = lb })
                                .Select(combined => new { Land = combined.Land, Country = combined.Country, LandBuilding = combined.LandBuilding }).ToList();
            /*
            Land land = landQuery.Land.Fi;

            War war = warsQuery.War;
            Land landFrom = warsQuery.LandFrom;
            Land landTo = warsQuery.LandTo;
            Country countryFrom = warsQuery.CountryFrom;
            Country countryTo = warsQuery.CountryTo;

            WarInfoViewModel warInfoViewModel = new WarInfoViewModel
            {
                War = war,
                LandFrom = landFrom,
                LandTo = landTo,
                CountryFrom = countryFrom,
                CountryTo = countryTo
            };

            return Json(JsonSerializer.Serialize(warInfoViewModel));
            */
            return null;
        }
    }
}
