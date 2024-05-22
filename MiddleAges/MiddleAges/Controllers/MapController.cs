using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Models;
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
            return View("Map", player);
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
            Land land = _context.Lands.FirstOrDefault(k => k.LandId == id);
            if (land == null)
            {
                return Json("NotFound");
            }
            return Json(JsonSerializer.Serialize(land));
        }
        public JsonResult FetchLandColors()
        {
            var landIdColorPairList = _context.Lands.Include(l => l.Country)
                .Select(l => new {LandId = l.LandId, Color = l.Country.Color});
            return Json(landIdColorPairList);
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
    }
}
