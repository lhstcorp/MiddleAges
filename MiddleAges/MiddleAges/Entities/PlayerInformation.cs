using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Entities
{
    public class PlayerInformation
    {
        [Key]
        public Guid PlayerInformationId { get; set; }
        public string PlayerId { get; set; }
        public string Vk { get; set; }
        public string Telegram { get; set; }
        public string Discord { get; set; }
        public string Facebook { get; set; }
        public string Description { get; set; }
        public Player Player { get; set; }
    }
}
