﻿using Microsoft.AspNetCore.Authorization;
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
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    [Authorize]
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
            //player.ImageURL = Regex.Match(selectedImageId, @"\d+").Value + ".webp";
            player.ImageURL = Path.GetFileName(selectedImageId);
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

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            bool ret = false;
            long maxFileSize = 512 * 1024;

            var permittedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" };

            if (avatarFile == null || avatarFile.Length == 0)
            {
                ret = true; // Файл не выбран
            }

            if (avatarFile.Length > maxFileSize)
            {
                ret = true; //"Размер файла не должен превышать 512 KB.";
            }

            var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();

            // Проверяем расширение файла
            if (!permittedExtensions.Contains(extension))
            {
                ret = true; // "Допустимые форматы: png, jpg, jpeg, webp.";
            }

            Player player = await _userManager.GetUserAsync(HttpContext.User);

            if (player.Money < 300)
            {
                ret = true; // Not enough money
            }

            // Путь для сохранения загруженного изображения
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/avatars");

            // Создаем директорию, если её нет
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (!ret)
            { 
                // Создаем уникальное имя файла, можно использовать идентификатор пользователя, чтобы избежать коллизий
                var fileName = $"{player.Id}_{Path.GetFileName(avatarFile.FileName)}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Сохраняем файл на диск
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // Обновляем поле ImageURL для пользователя, сохраняя путь к новому аватару
                player.ImageURL = $"{fileName}";
                player.Money -= 300;
                _context.Update(player);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // Возвращаем пользователя обратно к настройкам
        }
    }
}
