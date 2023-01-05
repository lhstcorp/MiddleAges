using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class Player
    {
        [Key]
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public long Exp { get; set; }
        public int Lvl { get; set; }
        public long Money { get; set; }
        public string ImageURL { get; set; }
        public int CurrentRegion { get; set; }
    }
}
