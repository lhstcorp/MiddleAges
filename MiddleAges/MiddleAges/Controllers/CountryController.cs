using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Enums;
using MiddleAges.Migrations;
using MiddleAges.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MiddleAges.Controllers
{
    public class CountryController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public CountryController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (country?.Name == "Independent lands")
            {
                return View("FoundCountry", player);
            }
            else
            {
                List<Land> countryLands = await _context.Lands.Where(l => l.CountryId == country.CountryId).ToListAsync();
                List<Country> otherCountries = await _context.Countries.Where(c => c.CountryId != country.CountryId && c.Name != "Independent lands").ToListAsync();
                List<Player> otherRulers = await _context.Players.Include(p => p.Land).ThenInclude(l => l.Country).Where(p => p.Id != country.RulerId && p.Land.CountryId == country.CountryId).ToListAsync();
                List<Law> laws = await _context.Laws.Where(l => l.CountryId == country.CountryId).ToListAsync();
                List<Land> landsToTranfer = await _context.Lands.Where(l => l.CountryId == country.CountryId && l.LandId != country.CapitalId).ToListAsync();

                Country independentCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Name == "Independent lands");

                var borderLandsQuery = await _context.Lands
                                                .Include(l => l.Country)
                                                .Join(_context.BorderLands,
                                                        bll => bll.LandId,
                                                        bl => bl.BorderLandId,                                            
                                                        (bll, bl) => new { BLand = bll, BorderLand = bl })
                                                .Where(combined => combined.BLand.CountryId != independentCountry.CountryId 
                                                               &&  combined.BLand.CountryId != country.CountryId)
                                                .Join(_context.Lands,
                                                        combined => combined.BorderLand.LandId,
                                                        l => l.LandId,
                                                        (combined, l) => new { combined.BLand, combined.BorderLand, Land = l })
                                                .Join(_context.Countries,
                                                        combined => combined.Land.CountryId,
                                                        c => c.CountryId,
                                                        (combined, c) => new { combined.BLand, combined.BorderLand, combined.Land, Country = c })
                                                .Where(combined => combined.Land.CountryId == country.CountryId)
                                                .Select(combined => new { BLand = combined.BLand, BorderLand = combined.BorderLand, Land = combined.Land, Country = combined.Country }).ToListAsync();

                List<BorderLand> borderLands = borderLandsQuery.Select(q => q.BorderLand).ToList();

                List<Unit> countryUnits = await GetCountryUnits(country.CountryId);

                var countryInfoViewModel = new CountryInfoViewModel
                {
                    Country = country,
                    Lands = countryLands,
                    Ruler = country.Ruler,
                    OtherCountries = otherCountries,
                    OtherRulers = otherRulers,
                    Laws = laws,
                    LandsToTranfer = landsToTranfer,
                    BorderLands = borderLands,
                    PeasantsCount = countryUnits.Where(u => u.Type == (int)UnitType.Peasant).Sum(u => u.Count),
                    SoldiersCount = countryUnits.Where(u => u.Type == (int)UnitType.Soldier).Sum(u => u.Count),
                    LordsCount = countryUnits.GroupBy(u => u.PlayerId).Count()
                };
                return View("Country", countryInfoViewModel);
            }     
        }
        [HttpPost]
        public async Task<IActionResult> FoundState(string playerId, string countryname, string countrycolor)
        {
            Player player = _context.Players.FirstOrDefault(k => k.Id == playerId);
            Country existedCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryname);
            if (player?.Money               >= 10000
             && existedCountry              == null
             && countryname                 != "IndependentLands"
             && countryname.Length          <= 22)
            {
                player.Money -= 10000;
                Country country = new Country();
                country.Name = countryname;
                country.RulerId = player.Id;
                country.Color = countrycolor;
                country.CapitalId = player.CurrentLand;
                if (country.Name == "")
                {
                    country.Name = player.CurrentLand + "state";
                }
                _context.Update(player);
                _context.Update(country);
                await _context.SaveChangesAsync();
                Land land = _context.Lands.FirstOrDefault(k => k.LandId == player.CurrentLand);
                land.CountryId = country.CountryId;
                _context.Update(land);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }
        [HttpPost]
        public async Task<IActionResult> Rename(string newName)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.Renaming;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.Name;
                law.Value1 = newName;
                _context.Update(law);
                country.Name = newName;
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }
        [HttpPost]
        public async Task<IActionResult> Recolor(string newColor)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.Recoloring;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.Color;
                law.Value1 = newColor;
                _context.Update(law);
                country.Color = newColor;
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCapital(string newcapital)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.ChangingCapital;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.CapitalId;
                law.Value1 = newcapital;
                _context.Update(law);
                country.CapitalId = newcapital;
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }
        [HttpPost]
        public async Task<IActionResult> TransferLand(string transferLand, string toCountry)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                Land selectedTransferLand = await _context.Lands.FirstOrDefaultAsync(l => l.LandId == transferLand);
                Country selectedCountryTo = await _context.Countries.FirstOrDefaultAsync(c => c.Name == toCountry);
                if (selectedTransferLand.LandId != ""
                 && selectedCountryTo.CountryId != Guid.Empty
                 && selectedTransferLand.CountryId == country.CountryId)
                { 
                    Law law = new Law();
                    law.CountryId = country.CountryId;
                    law.PlayerId = player.Id;
                    law.Type = (int)LawType.TransferingLand;
                    law.PublishingDateTime = DateTime.UtcNow;
                    law.Value1 = transferLand;
                    law.Value2 = toCountry;
                    _context.Update(law);
                    selectedTransferLand.CountryId = selectedCountryTo.CountryId;
                    _context.Update(selectedTransferLand);
                    await _context.SaveChangesAsync();
                }
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRuler(string newRulerName)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                Player newRuler = await _context.Players.FirstOrDefaultAsync(p => p.UserName == newRulerName);
                if (newRuler.Id != "")
                {
                    Law law = new Law();
                    law.CountryId = country.CountryId;
                    law.PlayerId = player.Id;
                    law.Type = (int)LawType.ChangingRuler;
                    law.PublishingDateTime = DateTime.UtcNow;                    
                    law.Value1 = newRulerName;
                    law.Value2 = country.RulerId;
                    _context.Update(law);
                    country.RulerId = newRuler.Id;
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> SetTaxes(string landId, int taxValue)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(l => l.LandId == landId);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(c => c.CountryId == land.CountryId);

            if (player.Id == country.RulerId)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.SetLandTaxes;
                law.PublishingDateTime = DateTime.UtcNow;                
                law.Value1 = taxValue.ToString();
                law.Value2 = landId;
                _context.Add(law);

                land.Taxes = taxValue;
                _context.Update(land);

                await _context.SaveChangesAsync();               
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> DeclareWar(string warCombination)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            if (player.Id == country.RulerId)
            {
                string[] lands = warCombination.Split(" - ");

                Land landFrom = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == lands[0]);
                Land landTo = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == lands[1]);

                War warCheck = await _context.Wars.FirstOrDefaultAsync(w => (w.LandIdFrom == landFrom.LandId
                                                                         || w.LandIdTo   == landFrom.LandId
                                                                         || w.LandIdFrom == landTo.LandId
                                                                         || w.LandIdTo   == landTo.LandId)
                                                                         && w.IsEnded    == false);

                if (landFrom != null
                 && landTo != null
                 && landFrom.CountryId == country.CountryId
                 && landTo.CountryId != country.CountryId
                 && warCheck == null)
                {
                    Law law = new Law();
                    law.CountryId = country.CountryId;
                    law.PlayerId = player.Id;
                    law.Type = (int)LawType.DeclaringWar;
                    law.PublishingDateTime = DateTime.UtcNow;
                    law.Value1 = landFrom.LandId;
                    law.Value2 = landTo.LandId;
                    _context.Update(law);
                                        
                    War war = new War();
                    war.LandIdFrom = landFrom.LandId;
                    war.LandIdTo = landTo.LandId;
                    war.StartDateTime = DateTime.UtcNow.AddHours(24);

                    _context.Update(war);
                    
                    await _context.SaveChangesAsync();
                }
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> DisbandCountry()
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            Country independentCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Name == "Independent lands");
            List<Land> countryLands = await _context.Lands.Where(l => l.CountryId == country.CountryId).ToListAsync();
            if (player.Id == country.RulerId)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.Disbanding;
                law.PublishingDateTime = DateTime.UtcNow;
                _context.Update(law);
                for (int i = 0; i < countryLands.Count; i++)
                {
                    countryLands[i].CountryId = independentCountry.CountryId;
                    _context.Update(countryLands[i]);
                }
                _context.Remove(country);
                await _context.SaveChangesAsync();
            }
            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        private async Task<List<Unit>> GetCountryUnits(Guid countryId)
        {
            List<Unit> countryUnits = await _context.Units
                                                .Join(_context.Lands,
                                                    u => u.LandId,
                                                    l => l.LandId,
                                                    (u, l) => new { Unit = u, Land = l })
                                                .Where(combined => combined.Land.CountryId == countryId).Select(q => q.Unit).ToListAsync();

            return countryUnits;
        }
    }
}
