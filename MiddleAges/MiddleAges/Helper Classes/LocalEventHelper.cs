using MiddleAges.Temporary_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges.HelperClasses
{
    public static class LocalEventHelper
    {      
        public static List<LocalEvent> GetAllLocalEvent()
        {
            List<LocalEvent> localEvents = new List<LocalEvent>();

            localEvents.Add(new LocalEvent(
                1,
                "Wolves don't sleep",
                "My lord, a pack of wolves has appeared near your vicinity. The people of your remote village are asking for your help to protect their livestock.",
                1,
                "Send soldiers to comb the forest!",
                new[] { 3.00, 0.00, 0.00, 10.00, -5.00 },
                new[] { 100.00, 0.00, 0.00, 100.00, 100.00},
                "Let them handle it themselves!",
                new[] { 0.00, -15.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                2,
                "Bandits in the forest",
                "My lord, we have heard rumors that our remote villages are being robbed by bandits. Our scouts reported that the bandits were holed up in the forest and preparing for a new raid.",
                1,
                "Kill them all!",
                new[] { 10.00, 0.00, 0.00, 10.00, -15.00 },
                new[] { 100.00, 0.00, 0.00, 100.00, 100.00 },
                "We won't be able to help them",
                new[] { 0.00, 0.00, 0.00, -15.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                3,
                "Knight Tournament",
                "A knight's tournament with good prize money will be held in the neighboring county. Perhaps we can send some of our knights?",
                1,
                "Send the best warrior to try his luck!",
                new[] { 10.00, 0.00, 0.00, 0.00, -1.00 },
                new[] { 10.00, 0.00, 0.00, 0.00, 100.00 },
                "Every warrior counts for us!",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

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
