using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        private readonly ILogger<MapController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public MapController(ILogger<MapController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);

            MapSelectedLandViewModel mapSelectedLandViewModel = GetMapSelectedLandViewModel(player.CurrentLand).Result;

            return View("Map", mapSelectedLandViewModel);
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

        public JsonResult GetLandDataById(string id)
        {            
            if (id == null)
            {
                return Json("NotFound");
            }

            MapSelectedLandViewModel mapSelectedLandViewModel = GetMapSelectedLandViewModel(id).Result;

            return Json(JsonSerializer.Serialize(mapSelectedLandViewModel));
        }

        public async Task<MapSelectedLandViewModel> GetMapSelectedLandViewModel(string id)
        {
            Land land = await _context.Lands.Include(l => l.Country).ThenInclude(l => l.Ruler)
                                      .Include(l => l.Governor).FirstOrDefaultAsync(l => l.LandId == id);

            var player = await _userManager.GetUserAsync(HttpContext.User);

            MapSelectedLandViewModel mapSelectedLandViewModel = new MapSelectedLandViewModel();

            mapSelectedLandViewModel.Player = player;
            mapSelectedLandViewModel.Land = land;
            mapSelectedLandViewModel.Country = land.Country;
            mapSelectedLandViewModel.Governor = land.Governor;
            mapSelectedLandViewModel.Ruler = land.Country.Ruler;
            mapSelectedLandViewModel.Population = GetLandPopulation(land.LandId).Result;
            mapSelectedLandViewModel.LordsCount = GetLandLordsCount(land.LandId).Result;
            mapSelectedLandViewModel.ResidentsCount = GetLandResidentsCount(land.LandId).Result;
            mapSelectedLandViewModel.CountryLandsCount = await GetCountryLandsCount(land.CountryId);
            mapSelectedLandViewModel.LandBuildings = GetLandBuildings(land.LandId).Result;

            var userAgent = Request.Headers["User-Agent"].ToString();
            var deviceType = userAgent.Contains("Mobi") ? "Mobile" : "Desktop";

            mapSelectedLandViewModel.DeviceType = deviceType;

            mapSelectedLandViewModel.BorderWith = GetLandBorderWithList(land.LandId);

            return mapSelectedLandViewModel;
        }

        public JsonResult FetchLandColors()
        {
            var landIdColorPairList = _context.Lands.Include(l => l.Country)
                .Select(l => new {LandId = l.LandId, Color = l.Country.Color});
            return Json(landIdColorPairList);
        }

        public JsonResult FetchWars()
        {
            var warList = _context.Wars
                                .Where(w => w.IsEnded == false)
                                .Join(_context.Lands,
                                        w => w.LandIdFrom,
                                        lf => lf.LandId,
                                        (w, lf) => new { War = w, LandFrom = lf })
                                .Join(_context.Lands,
                                        combined => combined.War.LandIdTo,
                                        lt => lt.LandId,
                                        (combined, lt) => new { combined.War, combined.LandFrom, LandTo = lt });

            return Json(warList);
        }

        public JsonResult FetchPlayer()
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;
            player = _context.Players.Include(p => p.Land).FirstOrDefault(p => p.Id == player.Id);

            return Json(player);
        }

        public async Task<IActionResult> MoveToLand(string landId)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(a => a.LandId == landId);

            if (player != null
             && land != null
             && player.CurrentLand != land.LandId)
            {
                player.CurrentLand = land.LandId;

                _context.Update(player);

                await _context.SaveChangesAsync();

                result = "Ok";
            }

            return Json(JsonSerializer.Serialize(result));
        }

        private async Task<int> GetLandPopulation(string landId)
        {
            int population = await _context.Units.Where(u => u.LandId == landId 
                                                          && u.Type == (int)UnitType.Peasant).SumAsync(u => u.Count);

            return population;
        }

        private async Task<int> GetLandLordsCount(string landId)
        {
            int lordsCount = await _context.Players.Where(p => p.CurrentLand == landId).CountAsync();

            return lordsCount;
        }

        private async Task<int> GetLandResidentsCount(string landId)
        {
            int lordsCount = await _context.Players.Where(p => p.ResidenceLand == landId).CountAsync();

            return lordsCount;
        }

        private async Task<int> GetCountryLandsCount(Guid? countryId)
        {
            if (countryId == null) 
                return 0;

            int countryLandsCount = await _context.Lands.Where(l => l.CountryId == countryId).CountAsync();

            return countryLandsCount;
        }

        public async Task<IActionResult> SettleDown(string landId)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(a => a.LandId == landId);

            if (player != null
             && land != null
             && player.CurrentLand == land.LandId
             && player.ResidenceLand != land.LandId)
            {
                player.ResidenceLand = land.LandId;

                _context.Update(player);

                List<Unit> units = await _context.Units.Where(u => u.PlayerId == player.Id).ToListAsync();

                foreach (Unit u in units)
                {
                    u.LandId = land.LandId;
                    _context.Update(u);
                }

                List<Building> buildings = await _context.Buildings.Where(b => b.PlayerId == player.Id).ToListAsync();

                foreach (Building b in buildings)
                {
                    b.LandId = land.LandId;
                    _context.Update(b);
                }

                await _context.SaveChangesAsync();

                result = "Ok";
            }

            return Json(JsonSerializer.Serialize(result));
        }

        public async Task<IActionResult> StartAnUprising(string landId)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.Include(l => l.Country).FirstOrDefaultAsync(a => a.LandId == landId);
            War warCheck = await _context.Wars.FirstOrDefaultAsync(w => (w.LandIdFrom == landId
                                                                      || w.LandIdTo == landId)
                                                                      && w.IsEnded == false);

            if (player != null
             && land != null
             && player.CurrentLand == land.LandId
             && player.Money > 300
             && warCheck == null
             && land.Country.Name != "Independent lands")
            {
                player.Money -= 300;
                _context.Update(player);

                War war = new War();
                war.LandIdFrom = land.LandId;
                war.LandIdTo = land.LandId;
                war.StartDateTime = DateTime.UtcNow.AddHours(24);
                war.IsRevolt = true;
                war.RebelId = player.Id;

                _context.Update(war);

                await _context.SaveChangesAsync();

                result = "Ok";
            }

            return Json(JsonSerializer.Serialize(result));
        }

        //private string GetLandBorderWith(string landId)
        //{
        //    List<BorderLand> borderLands = _context.BorderLands.Where(bl => bl.LandId == landId).ToList();

        //    string borderLandsStr = "";

        //    for (int i = 0; i < borderLands.Count; i++)
        //    {
        //        if (borderLandsStr.Length > 0)
        //        {
        //            borderLandsStr += ", ";
        //        }

        //        borderLandsStr += borderLands[i].BorderLandId;
        //    }

        //    return borderLandsStr;
        //}

        private List<BorderLand> GetLandBorderWithList(string landId)
        {
            List<BorderLand> borderLands = _context.BorderLands.Where(bl => bl.LandId == landId).ToList();

            return borderLands;
        }

        private async Task<List<LandBuilding>> GetLandBuildings(string landId)
        {
            List<LandBuilding> landBuildings = await _context.LandBuildings.Where(lb => lb.LandId == landId).ToListAsync();
            
            return landBuildings;
        }

        public async Task<IActionResult> UpdateLandBuilding(string landId, string landBuildingType)
        {
            string result = "Error";

            Player player = await _userManager.GetUserAsync(HttpContext.User);
            LandBuilding landBuilding = await _context.LandBuildings.Include(lb => lb.Land).ThenInclude(l => l.Country).FirstOrDefaultAsync(lb => lb.LandId == landId && (int)lb.BuildingType == Convert.ToInt32(landBuildingType));
            
            double landBuildingPrice = GetLandBuildingPrice(landBuilding);

            if (landBuilding != null
             && landBuilding.Land.Money > landBuildingPrice
             && (landBuilding.Land.GovernorId == player.Id
              || landBuilding.Land.Country.RulerId == player.Id)) 
            {
                landBuilding.Lvl += 1;
                _context.Update(landBuilding);

                landBuilding.Land.Money -= landBuildingPrice;
                _context.Update(landBuilding.Land);

                result = "OK";

                await _context.SaveChangesAsync();
            }

            return Json(JsonSerializer.Serialize(result));
        }

        private double GetLandBuildingPrice(LandBuilding landBuilding) 
        {
            return CommonLogic.BaseLandBuildingPrice + 2 * (landBuilding.Lvl - 1);
        }

        public async Task<IActionResult> GetRulerAccess(string id)
        {
            bool result = false;

            if (id != null)
            {
                Player player = await _userManager.GetUserAsync(HttpContext.User);
                Land land = await _context.Lands.Include(l => l.Country).FirstOrDefaultAsync(l => l.LandId == id);

                if (land.GovernorId == player.Id
                 || land.Country.RulerId == player.Id)
                {
                    result = true;
                }
            }

            return Json(JsonSerializer.Serialize(result));
        }

        public JsonResult GetLandForTransferingMoney(string id)
        {
            if (id == null)
            {
                return Json("NotFound");
            }

            Land land = _context.Lands.FirstOrDefault(l => l.LandId == id);

            return Json(JsonSerializer.Serialize(land));
        }

        public async Task<IActionResult> TransferMoneyToCountry(string landId, string transferAmountValue)
        {
            string result = "Error";

            if (landId != null
             && transferAmountValue != null)
            {
                Player player = await _userManager.GetUserAsync(HttpContext.User);
                Land land = await _context.Lands.Include(l => l.Country).FirstOrDefaultAsync(l => l.LandId == landId);

                double transferAmount = Convert.ToDouble(transferAmountValue);

                if ((land.GovernorId == player.Id
                  || land.Country.RulerId == player.Id)
                 && land.Money >= transferAmount)
                {
                    result = "OK";

                    land.Money -= transferAmount;
                    land.Country.Money += transferAmount;
                    _context.Update(land);
                    
                    await _context.SaveChangesAsync();
                }
            }

            return Json(JsonSerializer.Serialize(result));
        }
    }
}
