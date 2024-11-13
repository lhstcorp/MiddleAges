using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class MapSelectedLandViewModel
    {
        public Player Player { get; set; }
        public Land Land { get; set; }
        public Country Country { get; set; }
        public Player Governor { get; set; }
        public Player Ruler { get; set; }
        public int Population { get; set; }
        public int LordsCount { get; set; }
        public int ResidentsCount { get; set; }
        public string BorderWith { get; set; }
        public string DeviceType { get; set; }
        public List<LandBuilding> LandBuildings { get; set; }
    }
}
