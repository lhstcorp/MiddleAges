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
                new[] { 0.00, -5.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                2,
                "Bandits in the forest",
                "My lord, we have heard rumors that our remote villages are being robbed by bandits. Our scouts reported that the bandits were holed up in the forest and preparing for a new raid.",
                1,
                "Kill them all!",
                new[] { 10.00, 0.00, 0.50, 10.00, -5.00 },
                new[] { 100.00, 0.00, 100.00, 50.00, 100.00 },
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
                "Your Majesty, a plague has gripped our city! People are dying and the disease is spreading at an alarming rate. The streets are empty and panic grips the population. We urgently need your guidance to prevent further spread of the infection and save as many lives as possible.",
                1,
                "Сall the plague doctor",
                new[] { -5.00, 0.00, 0.00, -5.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 50.00, 0.00 },
                "This doesn't concern us!",
                new[] { 0.00, 0.00, 0.00, -30.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                5,
                "Yarilo has come to you",
                "The god of fertility and agriculture, Yarilo, decided to help you. He is ready to bless our fields and gardens so that they present rich harvests. In return, he asks only for our respect and observance of his sacred rites. This can be a salvation for our people in difficult times.",
                1,
                "Money",
                new[] { 5.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
                "Recruits",
                new[] { 0.00, 4.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                6,
                "Fire danger",
                "Someone set the haystacks on fire. The flames have engulfed several blocks and people are fleeing their homes in panic. We need your leadership to mobilize all forces to fight the fire and save residents.",
                1,
                "Send your people to help",
                new[] { 0.00, 5.00, 0.10, 0.00, -2.00 },
                new[] { 0.00, 100.00, 100.00, 0.00, 100.00 },
                "Peasants can handle it themselves",
                new[] { 0.00, 0.00, 0.00, -20.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                7,
                "Meeting the Tsmok",
                "While hunting in the Dark forest, you came across Tsmok's lair. This is an ancient and dangerous creature, and its presence in our lands is alarming. We need your wise guidance to decide how to handle this situation.",
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
                "Goblin settled in the castle. He hides in dark corners and scares the servants. What should we do?",
                1,
                "Drive him away",
                new[] { -1.00, 0.00, 0.10, 5.00, 0.00 },
                new[] { 100.00, 0.00, 100.00, 100.00, 0.00 },
                "Listen his story",
                new[] { 5.00, 1.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                9,
                "Winter is Coming",
                "Neighbors report that a harsh winter is approaching. They predict severe frosts and snow storms that could cause serious damage to our lands. We need to prepare for this test and ensure supplies of food and fuel for our subjects.",
                1,
                "Send people for firewood",
                new[] { -1.00, 0.00, 0.10, 5.00, 0.00 },
                new[] { 100.00, 0.00, 100.00, 100.00, 0.00 },
                "They are trying to deceive us",
                new[] { 3.00, 0.00, 0.00, -10.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                10,
                "V for Vendetta",
                "There is an increase in crime in the country. Some advisers suggest executing criminals to restore order and set an example. Others believe that it is better to cover up crimes in order to avoid panic among the population. We need your wise decision to deal with this situation.",
                1,
                "Execute criminals",
                new[] { 0.00, 5.00, 0.50, 0.00, 0.00 },
                new[] { 0.00, 100.00, 100.00, 0.00, 0.00 },
                "Cover up for criminals",
                new[] { 10.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                11,
                "Time of the courtesans",
                "There is an increase in prostitution in the country. This causes anxiety among residents and undermines the moral fiber of society. We need your wise leadership to take action to reduce prostitution and restore morality.",
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
                "Speleologists discovered two caves. One of them is rumored to be full of ancient artifacts and treasures, but the path to it is dangerous and difficult. Another cave is said to lead to an underground lake of crystal clear water, but no one knows what lurks in its depths. Which one would you like to explore?",
                1,
                "Explore thunder cave",
                new[] { 10.00, 0.00, 0.00, 0.00, -3.00 },
                new[] { 100.00, 0.00, 0.00, 0.00, 100.00 },
                "Explore fire cave",
                new[] { 0.00, 7.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                13,
                "Hunger",
                "This year the harvest is bad and people are suffering from hunger. People are desperate for help and many families are on the brink of survival. We can help them",
                1,
                "Sacrifice your supplies",
                new[] { -10.00, 5.00, 0.00, 20.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 100.00, 0.00 },
                "Don't help them",
                new[] { 0.00, 0.00, 0.00, -50.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                14,
                "Falling birth rate",
                "Due to low incomes, the country's birth rate has fallen. Families cannot afford to raise children, and this is causing anxiety among the population. We need your leadership to develop measures to support and stimulate fertility.",
                1,
                "Support the population",
                new[] { -10.00, 20.00, 0.00, 0.00, 0.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
                "Don't help them",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                15,
                "From the Varangians to the Greeks",
                "Neighboring states want to build a new trade route and invite us to join. This route could greatly improve our economy and strengthen ties with our neighbors.",
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
                "Peasants ask you to build a memory lane for the heroes of the city. They want to honor those who gave their lives for our kingdom. Your decision will be an important step in creating a place where people can remember and be proud of their heroes.",
                1,
                "Build an alley in the town center",
                new[] { -3.00, 5.00, 0.00, 0.00, 3.00 },
                new[] { 100.00, 100.00, 0.00, 0.00, 50.00 },
                "Don't build it",
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
                17,
                "Mobilization",
                "Military adviser proposes mobilization to train troops. He believes that this will strengthen our army and increase combat readiness in the event of a threat.",
                1,
                "Mobilize",
                new[] { 0.00, -10.00, 0.00, 0.00, 10.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 100.00 },
                "Don't do it",
                new[] { 0.00, 2.00, 0.00, 0.00, 0.00 },
                new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
               18,
               "Olympic games",
               "Your country has been chosen to host the Olympic Games. This event could attract many tourists and investments, as well as strengthen international ties. Do you accept the offer?",
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
              "Walking along the seashore in the evening you met the beautiful Undine. She came out of the water and, as if enchanted, looked at you with her deep, sparkling eyes. Her voice, like the melody of the waves, offered you help in exchange for your trust and friendship.",
              1,
              "Give in to her temptation",
              new[] { 1.00, 3.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 100.00, 0.00, 0.00, 0.00 },
              "Give up intimacy",
              new[] { 3.00, 0.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              20,
              "Svyatogor",
              "Your army met the great hero Svyatogor. His mighty figure and legendary strength inspired the respect and admiration of your warriors. Svyatogor offered his help in the fight against enemies, and his presence could become a decisive factor in our victories.",
              1,
              "Ask him for help",
              new[] { -10.00, 0.00, 0.00, 0.00, 10.00 },
              new[] { 100.00, 0.00, 0.00, 0.00, 100.00 },
              "Pass by",
              new[] { 5.00, 0.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              21,
              "New chapter",
              "Finally, a day has come when you can relax and read a book. But which one should you choose? It may be worth paying attention to chronicles describing the glorious deeds of your ancestors, or to a book dedicated to the art of financial management and the prosperity of the kingdom.",
              1,
              "History book",
              new[] { 0.00, 1.00, 0.10, 0.00, 0.00 },
              new[] { 0.00, 100.00, 100.00, 0.00, 0.00 },
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
              "Gamayun brought the news that a storm is approaching. We need to prepare for her arrival and protect our lands from destruction. I propose to strengthen the castle walls and collect food supplies. It is also worth warning all residents to take shelter in safe places.",
              1,
              "Prepare for the storm",
              new[] { -1.00, 0.00, 0.20, -10.00, 0.00 },
              new[] { 100.00, 0.00, 0.00, 10.00, 0.00 },
              "I don't believe her!",
              new[] { 0.00, 0.00, 0.00, -50.00, 0.00 },
              new[] { 0.00, 0.00, 0.00, 50.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              24,
              "Rebirth of the firebird",
              "Your Majesty, the magical Firebird is on the verge of extinction. We should turn to the sages and alchemists of the kingdom so that they develop a plan to save her. It may be necessary to collect rare ingredients and perform a complex ritual. Your guidance and support will be key in this endeavor. We must also ensure the safety of our researchers by sending security guards with them.",
              1,
              "Help and set her on fire",
              new[] { 5.00, 5.00, 0.10, 0.00, -1.00 },
              new[] { 100.00, 100.00, 100.00, 0.00, 100.00 },
              "Don't save",
              new[] { 1.00, -5.00, 0.00, 0.00, 0.00 },
              new[] { 100.00, 50.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
              25,
              "In search of Indrik",
              "There are legends that Indrik, the king of all animals, walked across our lands thousands of years ago. However, this week, hunters at the tavern claimed to have seen it with their own eyes. We should send scouts to check these rumors. If Indrik really has returned, it could be of great significance for our kingdom.",
              1,
              "Go to excavation of remains",
              new[] { 2.00, 3.00, 0.20, 0.00, 0.00 },
              new[] { 100.00, 100.00, 100.00, 0.00, 0.00 },
              "Go in search of a living Indrik",
              new[] { 0.00, 5.00, 1.00, 0.00, -2.00 },
              new[] { 0.00, 100.00, 100.00, 0.00, 30.00 }));

            localEvents.Add(new LocalEvent(
             26,
             "Nightingale the Robber",
             "The Nightingale the Robber escaped from prison. We urgently need to assemble a detachment of the best warriors and send them in search of him. This criminal has proven himself dangerous many times before, and we cannot allow him to threaten our subjects again.",
             1,
             "Send troops after him immediately!",
             new[] { 10.00, 0.00, 0.50, 0.00, -10.00 },
             new[] { 50.00, 0.00, 100.00, 0.00, 80.00 },
             "Let him run away",
             new[] { -10.00, 0.00, 0.00, 0.00, 0.00 },
             new[] { 100.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             27,
             "He who has science does not need religion",
             "There is public prayer in church this evening. On the square at the same time, training in blacksmithing. You need to decide which of these events to attend. Your presence will be a symbol of support and inspiration to your subjects.",
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
             "Guests from Europe are increasingly coming to our countries. It's time for you to learn a foreign language to communicate with them, my lord! This will help build stronger connections and improve mutual understanding. Your desire for knowledge will become an example for everyone.",
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
             "On your way home, you notice a cat meowing pitifully. Perhaps she is lost or needs help. It's worth stopping and checking what happened to her. Your compassion and care could save her life.",
             1,
             "Take home",
             new[] { 0.00, 3.00, 0.00, 0.00, 0.00 },
             new[] { 0.00, 100.00, 0.00, 0.00, 0.00 },
             "Don't touch",
             new[] { 0.00, 0.00, 0.10, 0.00, 0.00 },
             new[] { 0.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             30,
             "The day of sacrifice",
             "The gods are angry and demand sacrifices. If we do not satisfy their demands, the country will suffer famine. We urgently need to decide what sacrifices to make to appease the gods and save our people.",
             1,
             "Sacrifice a peasant",
             new[] { 2.00, 0.00, 0.00, -1.00, 0.00 },
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


            localEvents.Add(new LocalEvent(
             33,
             "Right tributary of the Berezina",
             "Every year our rivers carry less and less water. If this isn't fixed, there will be a drought next year.",
             1,
             "Build an irrigation system",
             new[] { -2.00, 2.00, 0.20, 0.00, 0.00 },
             new[] { 100.00, 100.00, 100.00, 0.00, 0.00 },
             "Import water",
             new[] { -2.00, 0.00, 0.00, 2.00, 0.00 },
             new[] { 100.00, 0.00, 0.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
             34,
             "Learning is light and ignorance is darkness",
             "According to the latest data, our state lacks smart workers. I propose to build a school for our peasants",
             1,
             "Build a school",
             new[] { -1.00, 2.00, 0.50, 0.00, 0.00 },
             new[] { 100.00, 100.00, 100.00, 0.00, 0.00 },
             "Build additional barracks",
             new[] { -1.00, 0.00, 0.00, 0.00, 1.00 },
             new[] { 100.00, 0.00, 0.00, 0.00, 100.00 }));

            localEvents.Add(new LocalEvent(
            35,
            "Don't be a beast!",
            "People gathered in the square. They protest and are dissatisfied with their living and working conditions. They say that they are tired of enduring",
            1,
            "Improve living conditions",
            new[] { -2.00, 5.00, 0.00, 10.00, 0.00 },
            new[] { 100.00, 100.00, 0.00, 100.00, 0.00 },
            "Suppress protests",
            new[] { 0.00, -1.00, 0.10, -10.00, 1.00 },
            new[] { 0.00, 100.00, 100.00, 100.00, 100.00 }));

            localEvents.Add(new LocalEvent(
            36,
            "Traveling merchant",
            "A new man has appeared in town and is selling foreign things. He's kind of suspicious",
            1,
            "Let him trade",
            new[] { 2.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
            "Drive him out of town",
            new[] { 0.00, 0.00, 0.10, 0.00, 0.00 },
            new[] { 0.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            37,
            "Is this the holy grail?",
            "During excavations, your people discovered an ancient artifact from the beginning of our era. What do you want to do with him?",
            1,
            "Explore and leave in the museum",
            new[] { 0.00, 1.00, 0.20, 0.00, 0.00 },
            new[] { 0.00, 50.00, 100.00, 0.00, 0.00 },
            "Sell it",
            new[] { 5.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 50.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            38,
            "Like a bolt from the blue",
            "From the letter: my dear, my life is coming to an end. If you can, visit the old man one last time. I'll leave you a small gift. What I have managed to accumulate in my life.",
            1,
            "Go for inheritance",
            new[] { 5.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
            "Give it to your servants",
            new[] { 0.00, 5.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            39,
            "Deserters",
            "People came to our palace and asked for help and shelter. They are deserters and do not want to fight against their people, but are ready to become our soldiers.",
            1,
            "Settle them in the city",
            new[] { 0.00, 10.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 0.00 },
            "Drive deserters out of the city",
            new[] { 0.50, 0.00, 0.10, 0.00, 0.00 },
            new[] { 100.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            40,
            "Carriage for Cinderella",
            "This year nature promotes the growth of vegetables. Look at this pumpkin! It's the size of a carriage!",
            1,
            "Conduct research",
            new[] { 1.00, 0.00, 0.10, 0.00, 0.00 },
            new[] { 50.00, 0.00, 100.00, 0.00, 0.00 },
            "Have a competition for the biggest pumpkin",
            new[] { 0.00, 5.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            41,
            "Bread and salt",
            "We plan to plant a lot of grain crops. Need another mill",
            1,
            "Build a windmill",
            new[] { 1.00, 0.00, 0.00, 10.00, 0.00 },
            new[] { 50.00, 0.00, 0.00, 100.00, 0.00 },
            "Build a water mill",
            new[] { 1.00, 5.00, 0.00, 0.00, 0.00 },
            new[] { 50.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            42,
            "Death is just the beginning",
            "My lord, sad news has arrived. Your friend and great warrior has died. My condolences",
            1,
            "Organize a magnificent funeral",
            new[] { -3.00, 10.00, 0.00, 0.00, 1.00 },
            new[] { 100.00, 100.00, 0.00, 0.00, 50.00 },
            "Bury modestly",
            new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            43,
            "My home is my castle",
            "After the last attack on us, the castle walls fell into disrepair. Urgent repair required",
            1,
            "Fix the wall",
            new[] { -1.00, 5.00, 0.10, -10.00, 2.00 },
            new[] { 100.00, 50.00, 100.00, 50.00, 50.00 },
            "Leave the wall in its current state",
            new[] { 0.00, -3.00, 0.00, 0.00, -1.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 100.00 }));

            localEvents.Add(new LocalEvent(
            44,
            "The circus has left, the clowns remain",
            "The famous circus of freaks has come to our city. You were given tickets to the best seats",
            1,
            "Go to a show",
            new[] { 0.00, 0.00, 0.00, 1.00, 0.00 },
            new[] { 0.00, 0.00, 0.00, 100.00, 0.00 },
            "Prepare everyone to throw tomatoes",
            new[] { 1.00, 0.00, -0.50, 0.00, 0.00 },
            new[] { 10.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            45,
            "Don't spit in the well",
            "Peasants began to get rid of their garbage by throwing it into the river. The water has become undrinkable",
            1,
            "Prohibit throwing garbage into the river",
            new[] { 0.00, -1.00, 0.10, -1.00, 0.00 },
            new[] { 0.00, 100.00, 100.00, 100.00, 0.00 },
            "Purify water and create a waste disposal area",
            new[] { 0.00, 1.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            46,
            "Romeo and Juliet",
            "A conflict has arisen between the two most powerful families in the town",
            1,
            "Try to reconcile them",
            new[] { 0.00, 0.00, 0.50, 0.00, 0.00 },
            new[] { 0.00, 0.00, 100.00, 0.00, 0.00 },
            "Support one of the families",
            new[] { 0.00, 0.00, 0.00, -10.00, 2.00 },
            new[] { 0.00, 0.00, 0.00, 100.00, 100.00 }));

            localEvents.Add(new LocalEvent(
            47,
            "Templars?",
            "A secret society has been discovered in the city. They meet every Wednesday in the castle cellars",
            1,
            "Execute participants",
            new[] { 1.00, 0.00, 0.50, 0.00, 0.00 },
            new[] { 100.00, 0.00, 100.00, 0.00, 0.00 },
            "Spy on their activities",
            new[] { -1.00, 5.00, 0.00, 0.00, 0.00 },
            new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            48,
            "No corpse, no works",
            "The body of a man was found hanging in a noose in the city square. There is a poster on the floor: everyone is the arbiter of their own destiny",
            1,
            "Investigate a crime",
            new[] { -1.00, 0.00, 0.10, 0.00, 0.00 },
            new[] { 50.00, 0.00, 100.00, 0.00, 0.00 },
            "Ignore",
            new[] { 0.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 0.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            49,
            "People in the swamp",
            "Regular rains have led to the appearance of swamps in our region",
            1,
            "Drain the swamp",
            new[] { -1.00, 1.00, 0.10, 0.00, 0.00 },
            new[] { 50.00, 50.00, 100.00, 0.00, 0.00 },
            "Leave it to nature",
            new[] { 0.00, 0.00, 0.10, 0.00, 0.00 },
            new[] { 0.00, 0.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            50,
            "The truth is in the wine",
            "Our vineyards became sick and almost all dried up. There won't be enough wine reserves for next year",
            1,
            "Treat the vineyards",
            new[] { 0.00, 0.00, 0.10, -1.00, 0.00 },
            new[] { 0.00, 0.00, 100.00, 100.00, 0.00 },
            "Import wine",
            new[] { -2.00, 1.00, 0.00, 0.00, 0.00 },
            new[] { 100.00, 100.00, 0.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            51,
            "Another side of the moon",
            "For the first time in 100 years we are seeing a solar eclipse. People are terrified and waiting for the end of the world",
            1,
            "Reassure the population",
            new[] { 0.00, 5.00, 0.00, 0.00, 0.00 },
            new[] { 0.00, 100.00, 0.00, 0.00, 0.00 },
            "Conduct scientific research",
            new[] { 1.00, 0.00, 0.10, 0.00, 0.00 },
            new[] { 100.00, 0.00, 50.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            52,
            "Tremors",
            "The earthquake destroyed several houses on the outskirts of our town. Dozens of people were left without home",
            1,
            "Restore destroyed buildings",
            new[] { -5.00, 10.00, 0.00, 10.00, 0.00 },
            new[] { 100.00, 100.00, 0.00, 100.00, 0.00 },
            "Relocate people to safety",
            new[] { 0.00, 0.00, 0.10, -10.00, 0.00 },
            new[] { 0.00, 0.00, 100.00, 100.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            53,
            "Bards Festival",
            "Tomorrow our city will host the annual bard festival. Guests will arrive from different countries",
            1,
            "Visit the festival",
            new[] { 2.00, 0.00, 0.00, 0.00, 0.00 },
            new[] { 100.00, 0.00, 0.00, 0.00, 0.00 },
            "Perform yourself with a personal song",
            new[] { 0.00, 2.00, 0.10, 0.00, 0.00 },
            new[] { 0.00, 100.00, 100.00, 0.00, 0.00 }));

            localEvents.Add(new LocalEvent(
            54,
            "After the rain on Thursday",
            "Yesterday several fishermen went fishing and disappeared at sea. Their families are very worried",
            1,
            "Go in search",
            new[] { -1.00, 0.00, 0.00, 10.00, 0.00 },
            new[] { 100.00, 0.00, 0.00, 100.00, 0.00 },
            "Pray for their salvation",
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
