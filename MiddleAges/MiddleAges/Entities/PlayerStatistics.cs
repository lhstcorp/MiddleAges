using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class PlayerStatistics
    {
        public Guid PlayerStatisticsId { get; set; }
        public string PlayerId { get; set; }
        public int SoldiersKilled { get; set; }
        public int SoldiersLost { get; set; }
        public Player Player { get; set; }
    }
}
