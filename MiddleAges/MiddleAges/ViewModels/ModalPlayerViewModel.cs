using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class ModalPlayerViewModel
    {
        public Player Player { get; set; }
        public Rating Rating { get; set; }
        public PlayerInformation PlayerInformation { get; set; }
        public Land ResidenceLand { get; set; }
        public Country ResidenceCountry { get; set; }
        public Unit Peasants { get; set; }

    }
}
