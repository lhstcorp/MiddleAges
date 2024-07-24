using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiddleAges.Entities
{
    public class ChatMessage
    {
        [Key]
        public Guid MessageId { get; set; }
        public string PlayerId { get; set; }
        public string MessageValue { get; set; }
        public DateTime PublishingDateTime { get; set; } = DateTime.UtcNow;
        public int ChatRoomType { get; set; } //Check ChatRoomType enum
        public string ChatRoomId { get; set; } //Empty if ChatRoomType = Global
        public Player Player { get; set; }
    }
}
