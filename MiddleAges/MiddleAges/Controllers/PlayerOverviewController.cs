using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class PlayerOverviewController : Controller
    {
        private readonly ILogger<PlayerOverviewController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public PlayerOverviewController(ILogger<PlayerOverviewController> logger,
                                        ApplicationDbContext context,
                                        UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        // GET: PlayerOverviewController
        public ActionResult Index()
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;
            return View("_PlayerOverviewPartial", player);
        }
        // GET: PlayerOverviewController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: PlayerOverviewController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: PlayerOverviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: PlayerOverviewController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: PlayerOverviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: PlayerOverviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: PlayerOverviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Submit(FormModel model)
        //{
        //    // Получаем токен с клиента
        //    var recaptchaResponse = Request.Form["g-recaptcha-response"];

        //    // Проверяем reCAPTCHA
        //    var isCaptchaValid = await ValidateRecaptcha(recaptchaResponse);

        //    if (!isCaptchaValid)
        //    {
        //        ModelState.AddModelError("Recaptcha", "reCAPTCHA verification failed.");
        //        return View(model); // Показываем форму с ошибкой
        //    }

        //    // Если валидация пройдена, продолжаем обработку
        //    // Дальнейшая логика для создания аккаунта...
        //}

        //private async Task<bool> ValidateRecaptcha(string recaptchaResponse)
        //{
        //    var secret = "ВАШ_SECRET_KEY"; // Ваш secret key

        //    using (var httpClient = new HttpClient())
        //    {
        //        var content = new FormUrlEncodedContent(new[]
        //        {
        //    new KeyValuePair<string, string>("secret", secret),
        //    new KeyValuePair<string, string>("response", recaptchaResponse)
        //});

        //        var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        //        var jsonString = await response.Content.ReadAsStringAsync();

        //        dynamic jsonData = JsonConvert.DeserializeObject(jsonString);
        //        return jsonData.success == "true";
        //    }
        //}

    }
}
