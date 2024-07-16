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

            localEvents.Add(new LocalEvent(
                4,
                "Have you seen the plague doctor?",
                "There was a plague boom in a neighboring state. What shall we do, king?",
                1,
                "Сall the plague doctor",
                new[] { -5.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
                "This doesn't concern us!",
                new[] { 0.00, 0.00, 0.00, -10.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                5,
                "Yarilo has come to you",
                "The god of fertility and agriculture, Yarilo, decided to help you. What reward will you choose?",
                1,
                "Money",
                new[] { 5.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
                "Recruits",
                new[] { 0.00, 2.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                6,
                "Fire danger",
                "Someone set the haystacks on fire. The fire spread to the city. Shall we help the peasants?",
                1,
                "Send your people to help",
                new[] { 0.00, 1.00, 0.10, 0.00, 0.00 },
                new[] { 0.00, 100.00, 100.00, 0.00, 0.00 },
                "Peasants can handle it themselves",
                new[] { 0.00, 0.00, 0.00, -10.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                7,
                "Meeting the Tsmok",
                "While hunting in the Dark forest, you came across Tsmok's lair. Search it?",
                1,
                "Send soldiers to battle",
                new[] { 20.00, 5.00, 1.00, 0.00, -5.00 },
                new[] { 50.00, 50.00, 100.00, 0.00, 50.00 },
                "Try to tame Tsmok",
                new[] { 10.00, 1.00, 0.50, 0.00, 0.00 },
                new[] { 100.00, 50.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                8,
                "Goblin's history",
                "Goblin settled in the castle.What should we do?",
                1,
                "Drive him away",
                new[] { -1.00, 0.00, 0.10, 0.00, 0.00 },
                new[] { 100.00, 0.00, 100.00, 0.00, 0.00 },
                "Listen his story",
                new[] { 10.00, 1.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                9,
                "Winter is Coming",
                "Neighbors report that a harsh winter is approaching",
                1,
                "Send people for firewood",
                new[] { -1.00, 0.00, 0.10, 0.00, 0.00 },
                new[] { 100.00, 0.00, 100.00, 0.00, 0.00 },
                "They are trying to deceive us",
                new[] { 0.00, 0.00, 0.00, -10.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                10,
                "V for Vendetta",
                "There is an increase in crime in the country. Should this stop?",
                1,
                "Execute criminals",
                new[] { 0.00, -1.00, 1.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 100.00, 0.00, 0.00 },
                "Cover up for criminals",
                new[] { 10.00, 2.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                11,
                "Time of the courtesans",
                "There is an increase in prostitution in the country. Should this stop?",
                1,
                "Stop this",
                new[] { -2.00, 5.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
                "All professions are important",
                new[] { 5.00, 0.00, 0.00, -10.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                12,
                "Dungeon master",
                "Speleologists discovered two caves. Which one would you like to explore?",
                1,
                "Explore thunder cave",
                new[] { 10.00, 0.00, 0.00, 0.00, -1.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 100.00 },
                "Explore fire cave",
                new[] { 0.00, 3.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                13,
                "Hunger",
                "This year the harvest is bad and people are suffering from hunger. We can help them",
                1,
                "Sacrifice your supplies",
                new[] { -10.00, 5.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
                "Don't help them",
                new[] { 0.00, 0.00, 0.00, -50.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                14,
                "Falling birth rate",
                "Due to low incomes, the country's birth rate has fallen",
                1,
                "Support the population",
                new[] { -10.00, 10.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
                "Don't help them",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                15,
                "From the Varangians to the Greeks",
                "Neighboring states want to build a new trade route and invite us to join",
                1,
                "Get directions through your country",
                new[] { -10.00, 0.00, 0.00, 10.00, 6.00 },
                new[] { 100.00, 0.00, 0.00, 100.00, 100.00 },
                "Don't build it",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                16,
                "Memory lane",
                "Peasants ask you to build a memory lane for the heroes of the city",
                1,
                "Build an alley in the town center",
                new[] { -1.00, 5.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
                "Don't build it",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                17,
                "Mobilization",
                "Military adviser proposes mobilization to train troops",
                1,
                "Mobilize",
                new[] { 0.00, -10.00, 0.00, 0.00, 5.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 100.00 },
                "Don't do it",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
               18,
               "Olympic games",
               "Your country has been chosen to host the Olympic Games. Do you accept the offer?",
               1,
               "Hold the Olympic Games",
               new[] { -15.00, 10.00, 1.00, 50.00, 0.00 },
               new[] { 100.00, 100.00, 100.00, 50.00, 0.00 },
               "Refuse",
               new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
               new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              19,
              "Undine's call",
              "Walking along the seashore in the evening you met the beautiful Undine",
              1,
              "Give in to her temptation",
              new[] { 0.00, 1.00, 0.00, 0.00, 0.00 },
              new[] { 0.00, 100.00, 0.00, 0.00, 0.00 },
              "Give up intimacy",
              new[] { 1.00, 0.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              20,
              "Svyatogor",
              "Your army met the great hero Svyatogor",
              1,
              "Ask him for help",
              new[] { 0.00, 0.00, 0.00, 0.00, 10.00 },
              new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
              "Pass by",
              new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
              new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              21,
              "New chapter",
              "Finally, a day has come when you can relax and read a book. But which one should you choose?",
              1,
              "History book",
              new[] { 0.00, 0.00, 0.10, 0.00, 10.00 },
              new[] { 0.00, 0.00, 100.00, 0.00, 0.00 },
              "Economic book",
              new[] { 1.00, 0.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              22,
              "Ghoul attack",
              "Troubled times have begun. The ghouls went on a rampage and attacked the neighbors yesterday. We can catch them before tonight",
              1,
              "Let's take revenge on them",
              new[] { 0.00, 0.00, 0.50, 0.00, -5.00 },
              new[] { 0.00, 0.00, 100.00, 0.00, 50.00 },
              "I think they will be afraid to attack us",
              new[] { 0.00, 0.00, 0.00, -20.00, 0.00 },
              new[] { 0.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              23,
              "Gamayun's message",
              "Gamayun brought the news that a storm is approaching",
              1,
              "Prepare for the storm",
              new[] { -1.00, 0.00, 0.00, -10.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 10.00, 0.00 },
              "I don't believe her!",
              new[] { 0.00, 0.00, 0.00, -20.00, 0.00 },
              new[] { 0.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              24,
              "Rebirth of the firebird",
              "The firebird cannot be reborn! We have to help her",
              1,
              "Help and set her on fire",
              new[] { 0.00, 5.00, 1.00, 0.00, 0.00 },
              new[] { 0.00, 100.00, 100.00, 0.00, 0.00 },
              "Don't save",
              new[] { 1.00, -5.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 50.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              25,
              "In search of Indrik",
              "There are legends that Indrik walked across our lands thousands of years ago. But the hunters in the tavern said that they saw him with their own eyes that week",
              1,
              "Go to excavation of remains",
              new[] { 2.00, 0.00, 0.20, 0.00, 0.00 },
              new[] { 100.00, 0.00, 100.00, 0.00, 0.00 },
              "Go in search of a living Indrik",
              new[] { 0.00, 0.00, 1.00, 0.00, -2.00 },
              new[] { 0.00, 0.00, 100.00, 0.00, 30.00 }));

            localEvents.Add(new LocalEvent(
             26,
             "Nightingale the Robber",
             "The Nightingale the Robber escaped from prison. We need to find him urgently",
             1,
             "Send troops after him immediately!",
             new[] { 10.00, 0.00, 0.50, 0.00, -10.00 },
             new[] { 10.00, 0.00, 100.00, 0.00, 80.00 },
             "Let him run away",
             new[] { -10.00, 0.00, 0.00, 0.00, 0.00 },
             new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             27,
             "He who has science does not need religion",
             "There is public prayer in church this evening. On the square at the same time, training in blacksmithing",
             1,
             "Go to prayer",
             new[] { 0.00, 5.00, 0.00, 0.00, 0.00 },
             new[] { 0.00, 100.00, 0.00, 0.00, 0.00 },
             "Learn blacksmithing",
             new[] { -1.00, 0.00, 0.50, 0.00, 0.00 },
             new[] { 100.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             28,
             "English or Spanish?",
             "Guests from Europe are increasingly coming to our countries. It's time for you to learn a foreign language to communicate with them, my lord!",
             1,
             "English",
             new[] { 0.00, 1.00, 0.10, 0.00, 0.00 },
             new[] { 0.00, 50.00, 100.00, 0.00, 0.00 },
             "Spanish",
             new[] { 0.00, 1.00, 0.10, 0.00, 0.00 },
             new[] { 0.00, 50.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             29,
             "Tom&Jerry",
             "On your way home, you notice a cat meowing pitifully",
             1,
             "Take home",
             new[] { 0.00, 2.00, 0.00, 0.00, 0.00 },
             new[] { 0.00, 50.00, 0.00, 0.00, 0.00 },
             "Don't touch",
             new[] { 0.00, 0.00, 0.10, 0.00, 0.00 },
             new[] { 0.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             30,
             "The day of sacrifice",
             "The gods are angry and demand sacrifices. Otherwise there will be famine in the country",
             1,
             "Sacrifice a peasant",
             new[] { 4.00, 0.00, 0.00, -1.00, 0.00 },
             new[] { 100.00, 0.00, 0.00, 100.00, 0.00 },
             "Sacrifice an animal",
             new[] { 1.00, 0.00, 0.10, 0.00, 0.00 },
             new[] { 100.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             31,
             "Walpurgis Night",
             "You have received a letter from witches all over the country asking you to spend Walpurgis Night",
             1,
             "Prohibit the celebration",
             new[] { 0.00, 0.00, 0.20, -10.00, -1.00 },
             new[] { 0.00, 0.00, 100.00, 50.00, 50.00 },
             "Allow a celebration",
             new[] { 1.00, 2.00, 0.00, 0.00, 0.00 },
             new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             32,
             "It flowed down my mustache but didn’t get into my mouth",
             "Today we celebrate our town day",
             1,
             "Hold a feast",
             new[] { -2.00, 2.00, 0.20, 0.00, 0.00 },
             new[] { 100.00, 100.00, 100.00, 0.00, 0.00 },
             "Don't celebrate",
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
