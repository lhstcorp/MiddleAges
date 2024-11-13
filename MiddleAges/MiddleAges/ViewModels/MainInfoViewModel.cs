using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class MainInfoViewModel
    {
        public Player Player { get; set; }
        public PlayerAttribute PlayerAttribute { get; set; }
        public Land ResidenceLand { get; set; }
        public List<Unit> Units { get; set; }
        public List<PlayerLocalEvent> PlayerLocalEvents { get; set; }
        public double PeasantHourIncome { get; set; }
        public string DeviceType { get; set; }
    }
}
