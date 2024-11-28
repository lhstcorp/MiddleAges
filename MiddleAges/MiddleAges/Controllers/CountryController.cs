using Microsoft.AspNetCore.Http;
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
using System.IO;
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

                var userAgent = Request.Headers["User-Agent"].ToString();
                var deviceType = userAgent.Contains("Mobi") ? "Mobile" : "Desktop";

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
                    LordsCount = countryUnits.GroupBy(u => u.PlayerId).Count(),
                    DeviceType = deviceType
                };

                return View("Country", countryInfoViewModel);
            }     
        }

        [HttpPost]
        public async Task<IActionResult> FoundState(string playerId, string countryname, string countrycolor)
        {
            Player player = _context.Players.FirstOrDefault(k => k.Id == playerId);
            Country existedCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryname);
            if (player?.Money               >= 300
             && existedCountry              == null
             && countryname                 != "IndependentLands"
             && countryname.Length          <= 22)
            {
                player.Money -= 300;
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
            if (player.Id == country.RulerId
             && country.Money >= 10)
            {                
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.Renaming;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.Name;
                law.Value1 = newName;
                _context.Update(law);

                country.Money -= 10;
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
            if (player.Id == country.RulerId
             && country.Money >= 10)
            {                
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.Recoloring;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.Color;
                law.Value1 = newColor;
                _context.Update(law);

                country.Money -= 10;
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
            War war = await _context.Wars.FirstOrDefaultAsync(w => (w.LandIdFrom == country.CapitalId
                                                                 || w.LandIdTo == country.CapitalId)
                                                                 && w.IsEnded == false);
            if (player.Id == country.RulerId
             && war == null
             && country.Money >= 50)
            {                
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.ChangingCapital;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value2 = country.CapitalId;
                law.Value1 = newcapital;
                _context.Update(law);

                country.Money -= 50;
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
            if (player.Id == country.RulerId
             && country.Money >= 10)
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

                    country.Money -= 10;
                    _context.Update(country);

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
            if (player.Id == country.RulerId
             && country.Money >= 10)
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

                    country.Money -= 10;
                    country.RulerId = newRuler.Id;
                    _context.Update(country);

                    await _context.SaveChangesAsync();
                }
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> SetTaxes(string landId, int landTaxValue, int stateTaxValue)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(l => l.LandId == landId);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(c => c.CountryId == land.CountryId);

            if (player.Id == country.RulerId
             && landTaxValue >= 0 
             && landTaxValue <= 100
             && stateTaxValue >= 0
             && stateTaxValue <= 100
             && country.Money >= 1)
            {                
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.SetLandTaxes;
                law.PublishingDateTime = DateTime.UtcNow;                
                law.Value1 = landTaxValue.ToString() + " (" + stateTaxValue.ToString() + ")";
                law.Value2 = landId;
                _context.Add(law);

                land.LandTax = landTaxValue;
                land.CountryTax = stateTaxValue;
                _context.Update(land);

                country.Money -= 1;
                _context.Update(country);

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

            if (player.Id == country.RulerId
             && country.Money >= 50)
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

                    country.Money -= 50;
                    _context.Update(country);

                    await _context.SaveChangesAsync();
                }
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> AppointGovernor(string landId, string governorName)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == landId);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());
            Player governor = await _context.Players.FirstOrDefaultAsync(p => p.UserName == governorName);

            if (player.Id == country.RulerId
             && land.CountryId == country.CountryId
             && governor != null
             && country.Money >= 10)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.AppointGovernor;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value1 = governorName;
                law.Value2 = landId;
                _context.Update(law);

                country.Money -= 10;
                _context.Update(country);

                land.GovernorId = governor.Id;
                _context.Update(land);

                await _context.SaveChangesAsync();
            }

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Country"));
        }

        [HttpPost]
        public async Task<IActionResult> TransferMoney(string landId, double amount)
        {
            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == landId);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId == land.CountryId);

            if (player.Id == country.RulerId
             && land.CountryId == country.CountryId
             && country.Money >= amount
             && amount > 0)
            {
                Law law = new Law();
                law.CountryId = country.CountryId;
                law.PlayerId = player.Id;
                law.Type = (int)LawType.TransferingMoney;
                law.PublishingDateTime = DateTime.UtcNow;
                law.Value1 = amount.ToString();
                law.Value2 = landId;
                _context.Add(law);

                country.Money -= amount;
                _context.Update(country);

                land.Money += amount;
                _context.Update(land);
                
                await _context.SaveChangesAsync();
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
            if (player.Id == country.RulerId
             && country.Money >= 1)
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

        [HttpPost]
        public async Task<IActionResult> UploadBanner(IFormFile bannerFile)
        {
            bool ret = false;
            long maxFileSize = 512 * 1024;

            var permittedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" };

            if (bannerFile == null || bannerFile.Length == 0)
            {
                ret = true; // Файл не выбран
            }

            if (bannerFile.Length > maxFileSize)
            {
                ret = true; //"Размер файла не должен превышать 512 KB.";
            }

            var extension = Path.GetExtension(bannerFile.FileName).ToLowerInvariant();

            // Проверяем расширение файла
            if (!permittedExtensions.Contains(extension))
            {
                ret = true; // "Допустимые форматы: png, jpg, jpeg, webp.";
            }

            Player player = await _userManager.GetUserAsync(HttpContext.User);
            Land land = await _context.Lands.FirstOrDefaultAsync(k => k.LandId == player.CurrentLand);
            Country country = await _context.Countries.Include(r => r.Ruler).FirstOrDefaultAsync(k => k.CountryId.ToString() == land.CountryId.ToString());

            if (country.Money >= 50)
            {
                // Путь для сохранения загруженного изображения
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/country-banners");

                // Создаем директорию, если её нет
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                if (!ret)
                {
                    // Создаем уникальное имя файла, можно использовать идентификатор пользователя, чтобы избежать коллизий
                    var fileName = $"{country.CountryId}{Path.GetExtension(bannerFile.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Сохраняем файл на диск
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bannerFile.CopyToAsync(stream);
                    }

                    // Обновляем поле ImageURL для пользователя, сохраняя путь к новому аватару
                    country.ImageURL = $"{fileName}";
                    country.Money -= 50;
                    _context.Update(country);

                    Law law = new Law();
                    law.CountryId = country.CountryId;
                    law.PlayerId = player.Id;
                    law.Type = (int)LawType.ChangingBanner;
                    law.PublishingDateTime = DateTime.UtcNow;
                    _context.Update(law);

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index"); // Возвращаем пользователя обратно к настройкам
        }
    }
}
