using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    public class Law
    {
        [Key]
        public Guid LawId { get; set; } 
        public Guid CountryId { get; set; }
        public int Type { get; set; }
        public string PlayerId { get; set; }
        public DateTime PublishingDateTime { get; set; }
        public string NewValue { get; set; }
        public Country Country { get; set; }
        public Player Player { get; set; }
    }
}
