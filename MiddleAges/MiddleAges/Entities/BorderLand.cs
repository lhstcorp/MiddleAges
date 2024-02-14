using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    public class BorderLand
    {
        public string LandId { get; set; }
        public string BorderLandId { get; set; }
        public Land Land { get; set; }
    }
}
