using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Enums;
using MiddleAges.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MiddleAges.ViewModels;
using MiddleAges.Temporary_Entities;
using Microsoft.AspNetCore.Authorization;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public AdminController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitialLandBuildings()
        {
            List<Land> lands = _context.Lands.ToList();

            List<LandBuilding> landBuildings = _context.LandBuildings.ToList();            

            for (int i = 0; i < lands.Count; i++)
            {
                LandBuilding infrastructure = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Infrastructure);

                if (infrastructure == null)
                { 
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Infrastructure);
                }

                LandBuilding market = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Market);

                if (market == null)
                {
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Market);
                }

                LandBuilding fortification = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Fortification);

                if (fortification == null)
                {
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Fortification);
                }

                // -----

                LandDevelopmentShare landDevelopmentShare = _context.LandDevelopmentShares.FirstOrDefault(ld => ld.LandId == lands[i].LandId);

                if (landDevelopmentShare == null)
                {
                    CreateInitialLandDevelopmentShare(lands[i].LandId);
                }

                await _context.SaveChangesAsync();
            }            

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Admin"));
        }

        public void CreateLandBuilding(string landId, LandBuildingType landBuildingType)
        {
            LandBuilding landBuilding = new LandBuilding();

            landBuilding.LandId = landId;
            landBuilding.BuildingType = landBuildingType;
            landBuilding.Lvl = 1;

            _context.Add(landBuilding);
        }

        private void CreateInitialLandDevelopmentShare(string landId)
        {
            LandDevelopmentShare landDevelopmentShare = new LandDevelopmentShare();

            landDevelopmentShare.LandId = landId;
            landDevelopmentShare.InfrastructureShare = 1;
            landDevelopmentShare.MarketShare = 1;
            landDevelopmentShare.FortificationShare = 1;

            _context.Add(landDevelopmentShare);
        }

        [HttpPost]
        public async Task<IActionResult> CheckBorderLandMismatches()
        {
            List<BorderLandMismatch> borderLandMismatches = new List<BorderLandMismatch>();
            string greenSpan = "<span style=\"color: limegreen; font-weight: 700\">{0} | {1} | {2}</span>";

            List<Land> lands = await _context.Lands.ToListAsync();

            for (int i = 0; i < lands.Count; i++)
            {
                List<BorderLand> borderLandsFrom = await _context.BorderLands.Where(bl => bl.BorderLandId == lands[i].LandId).ToListAsync();
                List<BorderLand> borderLandsTo = await _context.BorderLands.Where(bl => bl.LandId == lands[i].LandId).ToListAsync();

                if (borderLandsFrom.Count != borderLandsTo.Count
                 || borderLandsFrom.Count == 0
                 || borderLandsTo.Count   == 0)
                {
                    BorderLandMismatch borderLandMismatch = new BorderLandMismatch
                    {
                        LandId = lands[i].LandId,
                        LandFromMentions = borderLandsFrom.Count,
                        LandToMentions = borderLandsTo.Count
                    };

                    borderLandMismatches.Add(borderLandMismatch);
                }
            }

            AdminViewModel adminViewModel = new AdminViewModel
            {
                BorderLandMismatches = borderLandMismatches
            };

            return await Task.Run<ActionResult>(() => View("Admin", adminViewModel));
        }

        [HttpPost]
        public async Task<IActionResult> ClearDatabase()
        {
            Country independentLandsCountry = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId.ToString() == CommonLogic.IndependentLandsCountryId);
            Player admin = await _context.Players.FirstOrDefaultAsync(p => p.Id == CommonLogic.AdminId);

            // Delete army records
            List<Army> armies = await _context.Armies.ToListAsync();
            
            foreach (Army army in armies)
            {
                _context.Remove(army);
            }

            // Delete war records
            List<War> wars = await _context.Wars.ToListAsync();

            foreach(War war in wars)
            {
                _context.Remove(war);
            }

            // Delete law records
            List<Law> laws = await _context.Laws.ToListAsync();

            foreach (Law law in laws)
            {
                _context.Remove(law);
            }

            // Delete country records
            List<Country> countries = await _context.Countries.Where(c => c.CountryId != independentLandsCountry.CountryId).ToListAsync();

            foreach (Country country in countries)
            {
                _context.Remove(country);
            }

            // Refresh land records
            List<Land> lands = await _context.Lands.Where(l => l.CountryId != independentLandsCountry.CountryId).ToListAsync();

            foreach (Land land in lands)
            {
                land.CountryId = independentLandsCountry.CountryId;

                _context.Update(land);
            }

            // Refresh land records
            List<LandBuilding> landBuildings = await _context.LandBuildings.Where(lb => lb.Lvl > 1).ToListAsync();

            foreach (LandBuilding landBuilding in landBuildings)
            {
                landBuilding.Lvl = 1;

                _context.Update(landBuilding);
            }

            // Delete rating records
            List<Rating> ratings = await _context.Ratings.ToListAsync();

            foreach (Rating rating in ratings)
            {
                _context.Remove(rating);
            }

            // Delete PlayerAttribute records
            List<PlayerAttribute> playerAttributes = await _context.PlayerAttributes.Where(pa => pa.PlayerId != admin.Id).ToListAsync();

            foreach (PlayerAttribute playerAttribute in playerAttributes)
            {
                _context.Remove(playerAttribute);
            }

            // Delete PlayerInformation records
            List<PlayerInformation> playerInformations = await _context.PlayerInformations.Where(pi => pi.PlayerId != admin.Id).ToListAsync();

            foreach (PlayerInformation playerInformation in playerInformations)
            {
                _context.Remove(playerInformation);
            }

            // Delete PlayerLocalEvent records
            List<PlayerLocalEvent> playerLocalEvents = await _context.PlayerLocalEvents.ToListAsync();

            foreach (PlayerLocalEvent playerLocalEvent in playerLocalEvents)
            {
                _context.Remove(playerLocalEvent);
            }

            // Delete PlayerStatistics records
            List<PlayerStatistics> playerStatistics = await _context.PlayerStatistics.Where(ps => ps.PlayerId != admin.Id).ToListAsync();

            foreach (PlayerStatistics ps in playerStatistics)
            {
                _context.Remove(ps);
            }

            // Delete unit records.
            List<Unit> units = await _context.Units.Where(u => u.PlayerId != admin.Id).ToListAsync();

            foreach (Unit unit in units)
            {
                _context.Remove(unit);
            }

            // Delete building records.
            List<Building> buildings = await _context.Buildings.Where(b => b.PlayerId != admin.Id).ToListAsync();

            foreach (Building building in buildings)
            {
                _context.Remove(building);
            }

            // Delete ChatMessage records
            List<ChatMessage> chatMessages = await _context.ChatMessages.ToListAsync();

            foreach (ChatMessage chatMessage in chatMessages)
            {
                _context.Remove(chatMessage);
            }

            // Delete player records.
            List<Player> players = await _context.Players.Where(b => b.Id != admin.Id).ToListAsync();

            foreach (Player player in players)
            {
                _context.Remove(player);
            }

            await _context.SaveChangesAsync();

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Admin"));
        }
    }
}
