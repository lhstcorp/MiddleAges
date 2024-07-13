using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class PlayerLocalEvent
    {
        [Key]
        public Guid LocalEventId { get; set; }
        public string PlayerId { get; set; }
        public int EventId { get; set; } //link to the specific LocalEvent
        public DateTime AssignedDateTime { get; set; }
        public Player Player { get; set; }
    }
}
