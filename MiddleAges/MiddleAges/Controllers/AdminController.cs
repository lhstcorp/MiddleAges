using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiddleAges.Data;
using MiddleAges.Entities;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Enums;
using MiddleAges.Models;
using System.Collections.Generic;

namespace MiddleAges.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;
        public AdminController(ILogger<MainController> logger, ApplicationDbContext context, UserManager<Player> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitialLandBuildings()
        {
            List<Land> lands = _context.Lands.ToList();

            List<LandBuilding> landBuildings = _context.LandBuildings.ToList();            

            for (int i = 0; i < lands.Count; i++)
            {
                LandBuilding infrastructure = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Infrastructure);

                if (infrastructure == null)
                { 
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Infrastructure);
                }

                LandBuilding market = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Market);

                if (market == null)
                {
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Market);
                }

                LandBuilding fortification = landBuildings.FirstOrDefault(lb => lb.LandId == lands[i].LandId && lb.BuildingType == LandBuildingType.Fortification);

                if (fortification == null)
                {
                    CreateLandBuilding(lands[i].LandId, LandBuildingType.Fortification);
                }

                // -----

                LandDevelopmentShare landDevelopmentShare = _context.LandDevelopmentShares.FirstOrDefault(ld => ld.LandId == lands[i].LandId);

                if (landDevelopmentShare == null)
                {
                    CreateInitialLandDevelopmentShare(lands[i].LandId);
                }

                await _context.SaveChangesAsync();
            }            

            return await Task.Run<ActionResult>(() => RedirectToAction("Index", "Admin"));
        }

        public void CreateLandBuilding(string landId, LandBuildingType landBuildingType)
        {
            LandBuilding landBuilding = new LandBuilding();

            landBuilding.LandId = landId;
            landBuilding.BuildingType = landBuildingType;
            landBuilding.Lvl = 1;

            _context.Add(landBuilding);
        }

        private void CreateInitialLandDevelopmentShare(string landId)
        {
            LandDevelopmentShare landDevelopmentShare = new LandDevelopmentShare();

            landDevelopmentShare.LandId = landId;
            landDevelopmentShare.InfrastructureShare = 1;
            landDevelopmentShare.MarketShare = 1;
            landDevelopmentShare.FortificationShare = 1;

            _context.Add(landDevelopmentShare);
        }
    }
}
