using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiddleAges.Entities
{
    public class War
    {
        [Key]
        public Guid WarId { get; set; }
        public string LandIdFrom { get; set; }
        public string LandIdTo { get; set; }
        public DateTime StartDateTime { get; set; }
        [DefaultValue(0)]
        public bool IsEnded { get; set; }
        [DefaultValue(0)]
        public int WarResult { get; set; }
        public bool IsRevolt { get; set; }
        public string RebelId { get; set; }
        public Player Player { get; set; }
    }
}
