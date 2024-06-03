using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MiddleAges.Enums;

namespace MiddleAges.Entities
{
    public class Army
    {
        [Key]
        public Guid ArmyId { get; set; }
        public string PlayerId { get; set; }
        public Guid WarId { get; set; }
        public string LandId { get; set; }
        public int SoldiersCount { get; set; }
        public int SoldiersKilled { get; set; }
        public int SoldiersLost { get; set; }
        public ArmySide Side { get; set; }
        public Player Player { get; set; }
        public War War { get; set; }
        public Land Land { get; set; }        
    }
}
