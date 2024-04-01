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

                var countryInfoViewModel = new CountryInfoViewModel
                {
                    Country = country,
                    Lands = countryLands,
                    Ruler = country.Ruler,
                    OtherCountries = otherCountries,
                    OtherRulers = otherRulers,
                    Laws = laws,
                    LandsToTranfer = landsToTranfer
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
