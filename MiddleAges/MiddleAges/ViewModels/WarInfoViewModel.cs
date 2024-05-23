using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class WarInfoViewModel
    {        
        public War War { get; set; }
        public Land LandFrom { get; set; }
        public Land LandTo { get; set; }
        public Country CountryFrom { get; set; }
        public Country CountryTo { get; set; }
        public Player Player { get; set; }
        public WarArmiesViewModel WarArmiesViewModel { get; set; }
    }
}
