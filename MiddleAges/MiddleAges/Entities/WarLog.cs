using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiddleAges.Entities
{
    public class WarLog
    {
        [Key]
        public Guid WarLogId { get; set; }
        public Guid WarId { get; set; }
        public string AttackEfficiency { get; set; }
        public string DefenceEfficiency { get; set; }
        public int AttackLosses { get; set; }
        public int DefenceLosses { get; set; }
        public DateTime CalculationTime { get; set; }
        public War War { get; set; }
    }
}
