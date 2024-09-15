using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiddleAges.Entities
{
    public class Land
    {
        [Key]
        public string LandId { get; set; } // same as Land Name
        public Nullable<Guid> CountryId { get; set; }
        [DefaultValue(0)]
        public int LandTax { get; set; }
        [DefaultValue(0)]
        public int CountryTax { get; set; }
        [DefaultValue(10000)]
        public double ProductionLimit { get; set; }
        [DefaultValue(0)]
        public double Money { get; set; }
        public Country Country { get; set; }
    }
}
