using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class Player : IdentityUser
    {
        [DefaultValue(0)]
        public long Exp { get; set; }
        [DefaultValue(1)]
        public int Lvl { get; set; }
        [DefaultValue(1000)]
        public double Money { get; set; }
        public string ImageURL { get; set; }
        public string CurrentLand { get; set; }
    }
}

