using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MiddleAges.Entities
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string CapitalId { get; set; } // LandId
        public string Color { get; set; }  
        public string RulerId { get; set; }
        [DefaultValue(0)]
        public double Money { get; set; }
        [JsonIgnore]
        public Player Ruler { get; set; }
    }
}
