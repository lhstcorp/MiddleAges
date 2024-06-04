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
        }

        public async Task<IActionResult> Index()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);
            player = await _context.Players.Include(p => p.Land).ThenInclude(l => l.Country).FirstOrDefaultAsync(p => p.Id == player.Id);

            MapSelectedLandViewModel mapSelectedLandViewModel = new MapSelectedLandViewModel();

            mapSelectedLandViewModel.Player = player;
            mapSelectedLandViewModel.Land = player.Land;
            mapSelectedLandViewModel.Country = player.Land.Country;
            mapSelectedLandViewModel.Population = await GetLandPopulation(player.CurrentLand);
            mapSelectedLandViewModel.LordsCount = await GetLandLordsCount(player.CurrentLand);

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
            Land land = _context.Lands.Include(c => c.Country).FirstOrDefault(l => l.LandId == id);

            if (land == null)
            {
                return Json("NotFound");
            }

            MapSelectedLandViewModel mapSelectedLandViewModel = new MapSelectedLandViewModel();

            mapSelectedLandViewModel.Land = land;
            mapSelectedLandViewModel.Country = land.Country;
            mapSelectedLandViewModel.Population = GetLandPopulation(land.LandId).Result;
            mapSelectedLandViewModel.LordsCount = GetLandLordsCount(land.LandId).Result;

            return Json(JsonSerializer.Serialize(mapSelectedLandViewModel));
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
    }
}
