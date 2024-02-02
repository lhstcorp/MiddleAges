using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    public class Building
    {
        [Key]
        public Guid BuildingId { get; set; }

        /// <summary>
        /// 1 - Estates
        /// 2 - Barracks
        /// </summary>
        public int Type { get; set; } 

        [DefaultValue(1)]
        public int Lvl { get; set; }

        public string PlayerId { get; set; }
        public Player Player { get; set; }   
    }
}
