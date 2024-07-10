using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.HelperClasses
{
    public static class LocalEventHelper
    {
        public struct LocalEvent
        {
            public int EventId;
            public string Title;
            public string Description;
            public string ImgUrl;
            public int Rarity;
            public string Option1Text;
            public double[] Option1Values;
            public double[] Option1Chances;
            public string Option2Text;
            public double[] Option2Values;
            public double[] Option2Chances;

            public LocalEvent(int eventId, string title, string description, string imgUrl, int rarity, string option1Text, double[] option1Values, double[] option1Chances, string option2Text, double[] option2Values, double[] option2Chances)
            {
                EventId = eventId;
                Title = title;
                Description = description;
                ImgUrl = imgUrl;
                Rarity = rarity;
                Option1Text = option1Text;
                Option1Values = option1Values;      //M R E P S
                Option1Chances = option1Chances;    // 100 - always true, 0 - never, 50 - 50% chance
                Option2Text = option2Text;
                Option2Values = option2Values;
                Option2Chances = option2Chances;
            }
        }

        public static List<LocalEvent> GetAllLocalEvent()
        {
            List<LocalEvent> localEvents = new List<LocalEvent>();

            localEvents.Add(new LocalEvent(
                1,
                "Wolves don't sleep",
                "My lord, a pack of wolves has appeared near your vicinity. The people of your remote village are asking for your help to protect their livestock.",
                "~/img/local-events/1.jpg",
                1,
                "Send soldiers to comb the forest!",
                new[] { 3.00, 0.00, 0.00, 10.00, -5.00 },
                new[] { 100.00, 0.00, 0.00, 100.00, 100.00},
                "Let them handle it themselves!",
                new[] { 0.00, -15.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            return localEvents;
        }

        public static LocalEvent GetLocalEventById(int eventId)
        {
            List<LocalEvent> localEvents = LocalEventHelper.GetAllLocalEvent();

            LocalEvent localEvent = localEvents.FirstOrDefault(le => le.EventId == eventId);

            return localEvent;
        }
    }
}
