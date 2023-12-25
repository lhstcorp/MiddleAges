using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    public class Land
    {
        [Key]
        public int LandId { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
    }
}
