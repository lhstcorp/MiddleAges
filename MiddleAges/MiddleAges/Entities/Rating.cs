using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public string PlayerId { get; set; }
        public int ExpPlace { get; set; }
        public int MoneyPlace { get; set; }
        public int WarPowerPlace { get; set; }
        public int TotalPlace { get; set; }
        public Player Player { get; set; }
    }
}
