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

namespace MiddleAges.Controllers
{
    public class WarsController : Controller
    {
        private readonly ILogger<WarsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public WarsController(ILogger<WarsController> logger,
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
                                            (combined, cf) => new { combined.War, combined.LandFrom, combined.LandTo, CountryFrom = cf})
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

            return View("Wars", warInfoViewModelList);
        }
        
        
    }
}
