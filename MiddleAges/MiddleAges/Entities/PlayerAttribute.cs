using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class PlayerAttribute
    {
        [Key]
        public Guid AttributeId { get; set; }
        public string PlayerId { get; set; }
        [DefaultValue(0)]
        public int Management { get; set; }
        [DefaultValue(0)]
        public int Warfare { get; set; }
        [DefaultValue(0)]
        public int Leadership { get; set; }
        public Player Player { get; set; }
    }
}
