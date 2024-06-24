using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Temporary_Entities
{
    public class RatingCalculatedPoints
    {
        public string PlayerId { get; set; }
        public double ExpPoints { get; set; }
        public double MoneyPoints { get; set; }
        public double WarPowerPoints { get; set; }
    }
}
