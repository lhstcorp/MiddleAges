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
using MiddleAges.Models;

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
                                    .Where(w => w.IsEnded == false)
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

            Player player = await _userManager.GetUserAsync(HttpContext.User);

            player = await _context.Players.Include(p => p.Land).ThenInclude(l => l.Country).FirstOrDefaultAsync(p => p.Id == player.Id);

            List<WarInfoViewModel> warInfoViewModelList = new List<WarInfoViewModel>();

            for (int i = 0; i < wars.Count; i++)
            {
                WarInfoViewModel warInfoViewModel = new WarInfoViewModel
                {
                    War = wars[i],
                    LandFrom = landsFrom[i],
                    LandTo = landsTo[i],
                    CountryFrom = countriesFrom[i],
                    CountryTo = countriesTo[i],
                    Player = player,
                    WarArmiesViewModel = this.GetWarArmiesViewModelByWarId(wars[i].WarId)
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

        public JsonResult GetArmiesByWarId(string id)
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;

            var query = _context.PlayerAttributes
                                    .Join(_context.Players,
                                        pa => pa.PlayerId,
                                        p => p.Id,
                                        (pa, p) => new { PlayerAttribute = pa, Player = p})
                                    .Join(_context.Armies,
                                        combined => combined.Player.Id,
                                        a => a.PlayerId,
                                        (combined, a) => new { combined.PlayerAttribute, combined.Player, Army = a})
                                    .Where(combined => combined.Army.WarId.ToString() == id)
                                    .Select(combined => new { PlayerAttribute = combined.PlayerAttribute, Player = combined.Player, Army = combined.Army }).ToList();

            List<Army> armies = query.Select(q => q.Army).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);
            List<PlayerAttribute> playerAttributes = query.Select(q => q.PlayerAttribute).ToList();
            List<PlayerAttribute> attackersPlayerAttributes = new List<PlayerAttribute>();
            List<PlayerAttribute> defendersPlayerAttributes = new List<PlayerAttribute>();

            foreach (Army army in attackersArmies)
            {
                attackersPlayerAttributes.Add(playerAttributes.FirstOrDefault(pa => pa.PlayerId == army.PlayerId));
            }

            foreach (Army army in defendersArmies)
            {
                defendersPlayerAttributes.Add(playerAttributes.FirstOrDefault(pa => pa.PlayerId == army.PlayerId));
            }

            double attackersArmyStrength = 100 + Math.Round(CommonLogic.GetAverageArmyWarfare(attackersArmies, attackersPlayerAttributes) * 2, 2);
            double defendersArmyStrength = 100 + Math.Round(CommonLogic.GetAverageArmyWarfare(defendersArmies, defendersPlayerAttributes) * 2, 2);

            var attackersFortificationStrengthQuery = _context.LandDevelopmentShares
                                                .Join(_context.Wars,
                                                    ld => ld.LandId,
                                                    w => w.LandIdFrom,
                                                    (ld, w) => new { LandDevelopmentShare = ld, War = w})
                                                .FirstOrDefault(q => q.War.WarId.ToString() == id);

            LandDevelopmentShare attackersLandDevelopmentShare = attackersFortificationStrengthQuery.LandDevelopmentShare;

            var defendersFortificationStrengthQuery = _context.LandDevelopmentShares
                                                .Join(_context.Wars,
                                                    ld => ld.LandId,
                                                    w => w.LandIdFrom,
                                                    (ld, w) => new { LandDevelopmentShare = ld, War = w })
                                                .FirstOrDefault(q => q.War.WarId.ToString() == id);

            LandDevelopmentShare defendersLandDevelopmentShare = defendersFortificationStrengthQuery.LandDevelopmentShare;

            LandDevelopmentShare maxFortificationLandDevelopmentShare = _context.LandDevelopmentShares.OrderByDescending(lds => lds.FortificationShare).FirstOrDefault();

            double attackersFortificationStrength = Math.Round(100 * CommonLogic.GetFortificationValue(attackersLandDevelopmentShare.FortificationShare, maxFortificationLandDevelopmentShare.FortificationShare));
            double defendersFortificationStrength = Math.Round(100 * CommonLogic.GetFortificationValue(defendersLandDevelopmentShare.FortificationShare, maxFortificationLandDevelopmentShare.FortificationShare));

            WarArmiesViewModel warArmiesViewModel = new WarArmiesViewModel
            {
                AttackersArmies = attackersArmies,
                DefendersArmies = defendersArmies,
                AttackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount),
                DefendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount),
                AttackersArmyStrength = attackersArmyStrength,
                DefendersArmyStrength = defendersArmyStrength,
                AttackersFortificationStrength = attackersFortificationStrength,
                DefendersFortificationStrength = defendersFortificationStrength,
                Player = player
            };

            return Json(JsonSerializer.Serialize(warArmiesViewModel));
        }

        public WarArmiesViewModel GetWarArmiesViewModelByWarId(Guid id)
        {
            Player player = _userManager.GetUserAsync(HttpContext.User).Result;
            List<Army> armies = _context.Armies.Include(a => a.Player).Where(a => a.WarId == id).ToList();
            List<Army> attackersArmies = armies.FindAll(a => a.Side == ArmySide.Attackers);
            List<Army> defendersArmies = armies.FindAll(a => a.Side == ArmySide.Defenders);

            WarArmiesViewModel warArmiesViewModel = new WarArmiesViewModel
            {
                AttackersArmies = attackersArmies,
                DefendersArmies = defendersArmies,
                AttackersSoldiersCount = attackersArmies.Sum(a => a.SoldiersCount),
                DefendersSoldiersCount = defendersArmies.Sum(a => a.SoldiersCount),
                Player = player
            };

            return warArmiesViewModel;
        }

        public async Task<IActionResult> SendTroops(string warId, string soldiersCount, string armySideValue)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            War war = await _context.Wars.FirstOrDefaultAsync(w => w.WarId.ToString() == warId);
            Unit soldierUnit = await _context.Units.FirstOrDefaultAsync(u => u.PlayerId == player.Id && u.Type == (int)UnitType.Soldier);

            List<Army> armiesInBattle = await _context.Armies.Where(a => a.PlayerId == player.Id).ToListAsync();

            int soldiersInUse = armiesInBattle.Sum(a => a.SoldiersCount);
            int soldiersEntered = Convert.ToInt32(soldiersCount);

            ArmySide armySide = armySideValue == "l" ? ArmySide.Attackers : ArmySide.Defenders;

            if (player != null
             && soldierUnit.Count - soldiersInUse >= soldiersEntered
             && war    != null
             && war.IsEnded == false
             && soldiersEntered > 0
             && (war.LandIdFrom == player.CurrentLand
              && armySide == ArmySide.Attackers
              || war.LandIdTo   == player.CurrentLand
              && armySide == ArmySide.Defenders))
            {              
                Army playerArmyInThisWar = await _context.Armies.FirstOrDefaultAsync(
                                                                    a => a.WarId.ToString() == warId 
                                                                      && a.Side == armySide
                                                                      && a.PlayerId == player.Id);

                if (playerArmyInThisWar == null)
                {
                    Army army = new Army
                    {
                        WarId = war.WarId,
                        PlayerId = player.Id,
                        SoldiersCount = soldiersEntered,
                        LandId = player.CurrentLand,
                        Side = armySide
                    };

                    _context.Update(army);
                }
                else
                {
                    playerArmyInThisWar.SoldiersCount += soldiersEntered;
                    _context.Update(playerArmyInThisWar);
                }
                
                await _context.SaveChangesAsync();

                result = "Ok";
            }
            
            return Json(JsonSerializer.Serialize(result));
        }

        public async Task<IActionResult> DisbandPlayerArmy(string armyId)
        {
            string result = "Error";
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Army army = await _context.Armies.FirstOrDefaultAsync(a => a.ArmyId.ToString() == armyId);

            if (player != null
             && army != null
             && player.Id == army.PlayerId)
            {
                _context.Remove(army);

                await _context.SaveChangesAsync();

                result = "Ok";
            }

            return Json(JsonSerializer.Serialize(result));
        }
    }
}
