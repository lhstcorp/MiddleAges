using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        [DefaultValue(0)]
        public long Exp { get; set; }
        [DefaultValue(1)]
        public int Lvl { get; set; }
        [DefaultValue(1000)]
        public long Money { get; set; }
        public string ImageURL { get; set; }
        public int CurrentRegion { get; set; }
    }
}
