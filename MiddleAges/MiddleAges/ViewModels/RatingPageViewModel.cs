using MiddleAges.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.ViewModels
{
    public class RatingPageViewModel
    {
        public List<Rating> Ratings { get; set; }
        public int LastPageNum { get; set; }
    }
}
