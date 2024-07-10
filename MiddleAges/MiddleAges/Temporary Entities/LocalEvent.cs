using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.Temporary_Entities
{
    public class LocalEvent
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rarity { get; set; }
        public string Option1Text { get; set; }
        public double[] Option1Values { get; set; }
        public double[] Option1Chances { get; set; }
        public string Option2Text { get; set; }
        public double[] Option2Values { get; set; }
        public double[] Option2Chances { get; set; }

        public LocalEvent(int eventId, string title, string description, int rarity, string option1Text, double[] option1Values, double[] option1Chances, string option2Text, double[] option2Values, double[] option2Chances)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Rarity = rarity;
            Option1Text = option1Text;
            Option1Values = option1Values;      //M R E P S
            Option1Chances = option1Chances;    // 100 - always true, 0 - never, 50 - 50% chance
            Option2Text = option2Text;
            Option2Values = option2Values;
            Option2Chances = option2Chances;
        }
    }
}
