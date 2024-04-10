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


                //List<Land> borderLands = new List<Land>();// = await _context.BorderLands.Include(l => l.Land).Where(bl => bl.CountryId == country.CountryId).ToListAsync();

                var result = await _context.BorderLands
                                    .Join(_context.Lands,
                                            bl => bl.LandId,
                                            l => l.LandId,
                                            (bl, l) => new { BorderLand = bl, Land = l })
                                    .Join(_context.Countries,
                                            combined => combined.Land.CountryId,
                                            c => c.CountryId,
                                            (combined, c) => new { combined.BorderLand, combined.Land, Country = c })
                                    .Where(combined => combined.Land.CountryId == country.CountryId)
                                    .Select(combined => new { BorderLand = combined.BorderLand, Land = combined.Land, Country = combined.Country }).ToListAsync();

                
                var borderLands = await _context.Lands
                                    .Join(_context.BorderLands,
                                            bll => bll.LandId,
                                            bl => bl.BorderLandId,                                            
                                            (bll, bl) => new { BLand = bll, BorderLand = bl })                    
                                    .Join(_context.Lands,
                                            combined => combined.BLand.LandId,
                                            l => l.LandId,
                                            (combined, l) => new { combined.BLand, combined.BorderLand, Land = l })
                                    .Join(_context.Countries,
                                            combined => combined.Land.CountryId,
                                            c => c.CountryId,
                                            (combined, c) => new { combined.BLand, combined.BorderLand, combined.Land, Country = c })
                                    .Where(combined => combined.Land.CountryId == country.CountryId)
                                    .Select(combined => new { BLand = combined.BLand, BorderLand = combined.BorderLand, Land = combined.Land, Country = combined.Country }).ToListAsync();
                
                var countryInfoViewModel = new CountryInfoViewModel
                {
                    Country = country,
                    Lands = countryLands,
                    Ruler = country.Ruler,
                    OtherCountries = otherCountries,
                    OtherRulers = otherRulers,
                    Laws = laws,
                    LandsToTranfer = landsToTranfer
                    //BorderLands = borderLands
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
             && countryname                 != "IndependentLands")
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
                 && selectedCountryTo.CountryId != Guid.Empty)
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
                    law.Value2 = country.RulerId;
                    law.Value1 = newRulerName;
                    _context.Update(law);
                    country.RulerId = newRuler.Id;
                    _context.Update(country);
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
    }
}
