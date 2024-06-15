using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class UnitsBuildingsViewModel
    {
        public Player Player { get; set; }
        public List<Unit> Units { get; set; }
        public List<Building> Buildings { get; set; }
    }
}
