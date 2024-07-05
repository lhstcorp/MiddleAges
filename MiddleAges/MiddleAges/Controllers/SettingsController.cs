using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Models;
using MiddleAges.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public SettingsController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            PlayerInformation playerInformation = await _context.PlayerInformations.FirstOrDefaultAsync(pi => pi.PlayerId == player.Id);

            SettingsViewModel settingsViewModel = new SettingsViewModel();
            settingsViewModel.Player = player;
            settingsViewModel.PlayerInformation = playerInformation;

            return View("Settings", settingsViewModel);
        }

        public async Task<JsonResult> UpdateAvatar(string selectedImageId)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            player.ImageURL = Regex.Match(selectedImageId, @"\d+").Value; ;
            _context.Update(player);
            await _context.SaveChangesAsync();
            return Json("OK");
        }

        public async Task<JsonResult> UpdateContactInformation(string vk, string tg, string ds, string fb, string description)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);

            PlayerInformation playerInformation = await _context.PlayerInformations.FirstOrDefaultAsync(pi => pi.PlayerId == player.Id);

            playerInformation.Vk = vk;
            playerInformation.Telegram = tg;
            playerInformation.Discord = ds;
            playerInformation.Facebook = fb;
            playerInformation.Description = description;
            _context.Update(playerInformation);

            await _context.SaveChangesAsync();

            return Json("OK");
        }
    }
}
