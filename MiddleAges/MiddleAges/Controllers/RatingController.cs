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
using MiddleAges.ViewModels;

namespace MiddleAges.Controllers
{
    public class RatingController : Controller
    {
        private readonly ILogger<RatingController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        const int ratingLinesPerPage = 50;
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
            List<Rating> rating = await _context.Ratings.Include(r => r.Player).Where(r => r.TotalPlace <= ratingLinesPerPage).OrderBy(r => r.TotalPlace).ToListAsync();
            return View("Rating", rating);
            //return View("Rating");
        }

        public async Task<IActionResult> GetRatingByCategoryAndPage(string category, string page)
        {
            int pageNumber = Convert.ToInt32(page) - 1;

            List<Rating> ratings = new List<Rating>();

            RatingPageViewModel ratingPageViewModel = new RatingPageViewModel();

            switch (category)
            {
                case "Total":
                    ratings = await _context.Ratings.Include(r => r.Player).Where(r => r.TotalPlace > pageNumber * ratingLinesPerPage
                                                                                   && r.TotalPlace <= pageNumber * ratingLinesPerPage + ratingLinesPerPage).OrderBy(r => r.TotalPlace).ToListAsync();
                    break;
                case "Exp":
                    ratings = await _context.Ratings.Include(r => r.Player).Where(r => r.ExpPlace > pageNumber * ratingLinesPerPage
                                                                                   && r.ExpPlace <= pageNumber * ratingLinesPerPage + ratingLinesPerPage).OrderBy(r => r.ExpPlace).ToListAsync();
                    break;
                case "Money":
                    ratings = await _context.Ratings.Include(r => r.Player).Where(r => r.MoneyPlace > pageNumber * ratingLinesPerPage
                                                                                   && r.MoneyPlace <= pageNumber * ratingLinesPerPage + ratingLinesPerPage).OrderBy(r => r.MoneyPlace).ToListAsync();
                    break;
                case "Power":
                    ratings = await _context.Ratings.Include(r => r.Player).Where(r => r.WarPowerPlace > pageNumber * ratingLinesPerPage
                                                                                   && r.WarPowerPlace <= pageNumber * ratingLinesPerPage + ratingLinesPerPage).OrderBy(r => r.WarPowerPlace).ToListAsync();
                    break;
            }

            ratingPageViewModel.Ratings = ratings;
            ratingPageViewModel.LastPageNum = await _context.Ratings.CountAsync() / ratingLinesPerPage + 1;

            return Json(JsonSerializer.Serialize(ratingPageViewModel));
        }

        public async Task<IActionResult> SearchRatingsByPlayerName(string playerName)
        {
            List<Rating> ratings = await _context.Ratings.Include(r => r.Player).Where(r => r.Player.UserName.Contains(playerName)).OrderBy(r => r.TotalPlace).ToListAsync();

            RatingPageViewModel ratingPageViewModel = new RatingPageViewModel();

            ratingPageViewModel.Ratings = ratings;
            ratingPageViewModel.LastPageNum = 1;

            return Json(JsonSerializer.Serialize(ratingPageViewModel));
        }
    }
}
