using MiddleAges.Entities;
using System.ComponentModel.DataAnnotations;
using System;

namespace MiddleAges.Entities
{
    public class LandDevelopmentShare
    {
        [Key]
        public Guid LandDevelopmentId { get; set; }
        public string LandId { get; set; }
        public double InfrastructureShare { get; set; }
        public double MarketShare { get; set; }
        public double FortificationShare { get; set; }
        public Land Land { get; set; }
    }
}