using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace MiddleAges.Controllers
{
    public class RatingController : Controller
    {
        private readonly ILogger<RatingController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public RatingController(ILogger<RatingController> logger,
                                        ApplicationDbContext context,
                                        UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        // GET: PlayerOverviewController
        public async Task<IActionResult> Index()
        {
            List<Rating> rating = await _context.Ratings.Include(r => r.Player).Where(r => r.TotalPlace <= 50).OrderBy(r => r.TotalPlace).ToListAsync();
            return View("Rating", rating);
        }

        public async Task<IActionResult> GetRatingByCategoryAndPage(string category, string page)
        {
            int pageNumber = Convert.ToInt32(page) - 1;

            List<Rating> rating = new List<Rating>();

            switch (category)
            {
                case "Exp":
                    rating = await _context.Ratings.Include(r => r.Player).Where(r => r.ExpPlace > pageNumber * 50
                                                                                   && r.ExpPlace <= pageNumber * 50 + 50).OrderBy(r => r.ExpPlace).ToListAsync();
                    break;
                case "Money":
                    rating = await _context.Ratings.Include(r => r.Player).Where(r => r.MoneyPlace > pageNumber * 50
                                                                                   && r.MoneyPlace <= pageNumber * 50 + 50).OrderBy(r => r.MoneyPlace).ToListAsync();
                    break;
                case "Power":
                    rating = await _context.Ratings.Include(r => r.Player).Where(r => r.WarPowerPlace > pageNumber * 50
                                                                                   && r.WarPowerPlace <= pageNumber * 50 + 50).OrderBy(r => r.WarPowerPlace).ToListAsync();
                    break;
            }

            return Json(JsonSerializer.Serialize(rating));
        }
    }
}
