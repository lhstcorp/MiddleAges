using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using MiddleAges.Enums;

namespace MiddleAges.Controllers
{
    [Authorize]
    public class WarController : Controller
    {
        private readonly ILogger<WarController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public WarController(ILogger<WarController> logger,
                                        ApplicationDbContext context,
                                        UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var warsQuery = await _context.Wars
                                    .Join(_context.Lands,
                                            w => w.LandIdFrom,
                                            lf => lf.LandId,
                                            (w, lf) => new { War = w, LandFrom = lf })
                                    .Join(_context.Lands,
                                            combined => combined.War.LandIdTo,
                                            lt => lt.LandId,
                                            (combined, lt) => new { combined.War, combined.LandFrom, LandTo = lt })
                                    .Join(_context.Countries,
                                            combined => combined.LandFrom.CountryId,
                                            cf => cf.CountryId,
                                            (combined, cf) => new { combined.War, combined.LandFrom, combined.LandTo, CountryFrom = cf })
                                    .Join(_context.Countries,
                                            combined => combined.LandTo.CountryId,
                                            ct => ct.CountryId,
                                            (combined, ct) => new { combined.War, combined.LandFrom, combined.LandTo, combined.CountryFrom, CountryTo = ct })
                                    .Select(combined => new { War = combined.War, LandFrom = combined.LandFrom, LandTo = combined.LandTo, CountryFrom = combined.CountryFrom, CountryTo = combined.CountryTo }).ToListAsync();

            List<War> wars = warsQuery.Select(q => q.War).ToList();
            List<Land> landsFrom = warsQuery.Select(q => q.LandFrom).ToList();
            List<Land> landsTo = warsQuery.Select(q => q.LandTo).ToList();
            List<Country> countriesFrom = warsQuery.Select(q => q.CountryFrom).ToList();
            List<Country> countriesTo = warsQuery.Select(q => q.CountryTo).ToList();

            List<WarInfoViewModel> warInfoViewModelList = new List<WarInfoViewModel>();

            for (int i = 0; i < wars.Count; i++)
            {
                WarInfoViewModel warInfoViewModel = new WarInfoViewModel
                {
                    War = wars[i],
                    LandFrom = landsFrom[i],
                    LandTo = landsTo[i],
                    CountryFrom = countriesFrom[i],
                    CountryTo = countriesTo[i]
                };

                warInfoViewModelList.Add(warInfoViewModel);
            }

            return View("War", warInfoViewModelList);
        }

        public JsonResult GetWarById(string id)
        {
            var warsQuery = _context.Wars
                                .Where(w => w.WarId == Guid.Parse(id))
                                .Join(_context.Lands,
                                        w => w.LandIdFrom,
                                        lf => lf.LandId,
                                        (w, lf) => new { War = w, LandFrom = lf })
                                .Join(_context.Lands,
                                        combined => combined.War.LandIdTo,
                                        lt => lt.LandId,
                                        (combined, lt) => new { combined.War, combined.LandFrom, LandTo = lt })
                                .Join(_context.Countries,
                                        combined => combined.LandFrom.CountryId,
                                        cf => cf.CountryId,
                                        (combined, cf) => new { combined.War, combined.LandFrom, combined.LandTo, CountryFrom = cf })
                                .Join(_context.Countries,
                                        combined => combined.LandTo.CountryId,
                                        ct => ct.CountryId,
                                        (combined, ct) => new { combined.War, combined.LandFrom, combined.LandTo, combined.CountryFrom, CountryTo = ct })
                                .Select(combined => new { War = combined.War, LandFrom = combined.LandFrom, LandTo = combined.LandTo, CountryFrom = combined.CountryFrom, CountryTo = combined.CountryTo }).FirstOrDefault();

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
        }

        public class SendTroopsResult
        {
            public Guid WarId { get; set; }
            public int SoldiersCount { get; set; }
        }

        public async Task<IActionResult> SendTroops(string warId, string soldiersCount)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            War war = await _context.Wars.FirstOrDefaultAsync(w => w.WarId.ToString() == warId);
            Unit soldierUnit = await _context.Units.FirstOrDefaultAsync(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Soldier);

            List<Army> armiesInBattle = await _context.Armies.Where(a => a.PlayerId == player.Id).ToListAsync();

            int soldiersInUse = armiesInBattle.Sum(a => a.SoldiersCount);
            int soldiersEntered = Convert.ToInt32(soldiersCount);

            if (player != null
             && soldierUnit.Count - soldiersInUse >= soldiersEntered
             && war    != null
             && war.IsEnded == false
             && soldiersEntered > 0)
            {
                Army army = new Army
                {
                    WarId = war.WarId,
                    PlayerId = player.Id,
                    SoldiersCount = soldiersEntered,
                    LandId =  player.CurrentLand,
                    Side = war.LandIdFrom == player.CurrentLand ? ArmySide.Attackers : ArmySide.Defenders
                };

                _context.Update(army);
                await _context.SaveChangesAsync();
                
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }
    }
}
