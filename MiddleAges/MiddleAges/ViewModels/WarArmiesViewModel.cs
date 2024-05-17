using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class WarArmiesViewModel
    {
        public List<Army> AttackersArmies { get; set; }
        public List<Army> DefendersArmies { get; set; }
        public int AttackersSoldiersCount { get; set; }
        public int DefendersSoldiersCount { get; set; }
    }
}
