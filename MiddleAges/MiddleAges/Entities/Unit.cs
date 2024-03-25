using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    /// <summary>
    /// Specific type of units.
    /// </summary>
    public class Unit
    {
        [Key]
        public Guid UnitId { get; set; }       
        public int Type { get; set; }
        public int Lvl { get; set; }
        public int Count { get; set; }           
        public string PlayerId { get; set; }   
        public Player Player { get; set; }   
    }
}
