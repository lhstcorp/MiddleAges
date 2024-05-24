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
        public int Population { get; set; }
        public int LordsCount { get; set; }
    }
}
