using Microsoft.EntityFrameworkCore;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Models;

namespace MiddleAges.Models
{
    public static class CommonLogic
    {
        public static string getBuildingNameByType(int _buildingType)
        {
            string buildingName = "";
            switch(_buildingType)
            {
                case 1:
                    buildingName = "Estate";
                    break;
                case 2:
                    buildingName = "Barracks";
                    break;
            }
            return buildingName;
        }
        public static string getUnitNameByType(int _unitType)
        {
            string unitName = "";
            switch (_unitType)
            {
                case 1:
                    unitName = "Peasant";
                    break;
                case 2:
                    unitName = "Soldier";
                    break;
            }
            return unitName;
        }
        public static int getUnitPrice(int unitType)
        {
            int unitPrice = unitType switch
            {
                (int)UnitType.Peasant =>  (int)UnitPrice.Peasant,
                (int)UnitType.Soldier =>  (int)UnitPrice.Soldier,
                _ => 0
            };
            return unitPrice;
        }

        public static int GetUnitLimit(int buildingLvl)
        {
            int unitLimit = 500 + (buildingLvl - 1) * 100;

            return unitLimit;
        }

        public static double getBuildingPrice(int buildingType, int buildingLvl)
        {
            double buildingPrice = buildingType switch
            {
                (int)BuildingType.Estate => (int)BuildingPrice.Estate * Math.Pow(2, buildingLvl - 1), //500..1000..2000..4000 
                (int)BuildingType.Barracks => (int)BuildingPrice.Barracks * Math.Pow(2, buildingLvl - 1),
                _ => 0
            };
            return buildingPrice;
        }

        public static string getLawDescriptionByType(int lawType)
        {
            string lawDescription = lawType switch
            {
                (int)LawType.Renaming => "Country was renamed from {0} to {1}",
                (int)LawType.Recoloring => "Country changed color from {0} to {1} ",
                (int)LawType.ChangingCapital => "Country changed capital from {0} to {1} ",
                (int)LawType.TransferingLand => "Country transfered the land {0} to country {1} ",
                (int)LawType.ChangingRuler => "The Ruler was changed from {0} to {1} ",
                (int)LawType.DeclaringWar => "Country declared war on {0} ",
                (int)LawType.Disbanding => "Country was disbanded",
                (int)LawType.SetLandTaxes => "{0} taxes were set at {1}%",
                _ => ""
            };
            return lawDescription;
        }

        public static string getLawNameByType(int lawType)
        {
            string lawName = lawType switch
            {
                (int)LawType.Renaming => "Renaming",
                (int)LawType.Recoloring => "Recoloring",
                (int)LawType.ChangingCapital => "ChangingCapital",
                (int)LawType.TransferingLand => "TransferingLand",
                (int)LawType.ChangingRuler => "ChangingRuler",
                (int)LawType.DeclaringWar => "DeclaringWar",
                (int)LawType.Disbanding => "Disbanding",
                (int)LawType.SetLandTaxes => "SetLandTaxes",
                _ => ""
            };
            return lawName;
        }

        public static int GetAvailAttrPoints(int playerLvl, PlayerAttribute playerAttribute)
        {
            int attrPointsCount = 3 * playerLvl - playerAttribute.Management - playerAttribute.Warfare - playerAttribute.Leadership;

            return attrPointsCount;
        }

        public static double GetAverageArmyWarfare(List<Army> armies, List<PlayerAttribute> playerAttributes)
        {
            double soldiersCount = armies.Sum(a => a.SoldiersCount);

            double armyPower = 0;

            for (int i = 0; i < armies.Count; i++)
            {
                armyPower += armies[i].SoldiersCount * playerAttributes.FirstOrDefault(pa => pa.PlayerId == armies[i].PlayerId).Warfare;
            }

            double averageArmyWarfare = 0;

            if (soldiersCount > 0)
            {
                averageArmyWarfare = armyPower / soldiersCount;
            }

            return averageArmyWarfare;
        }

        public static long GetRequiredExpByLvl(long lvl)
        {
            long requiredExp = Convert.ToInt32(Math.Floor(Math.Pow(1.4, lvl)));

            return requiredExp;
        }

        public static string getRandomMapLandId()
        {
            List<string> landIds = new List<string>();
            landIds.Add("Aktsyabrski");
            landIds.Add("Ashmjany");
            landIds.Add("Asipovichy");
            landIds.Add("Astravets");
            landIds.Add("Babruisk");
            landIds.Add("Baranavichy");
            landIds.Add("Barysau");
            landIds.Add("Berazino");
            landIds.Add("Beshankovichy");
            landIds.Add("Bierastavica");
            landIds.Add("Brahin");
            landIds.Add("Braslau");
            landIds.Add("Brest");
            landIds.Add("Buda-Kashalyova");
            landIds.Add("Byalynichy");
            landIds.Add("Byaroza");
            landIds.Add("Byhau");
            landIds.Add("Chachersk");
            landIds.Add("Chashniki");
            landIds.Add("Chausy");
            landIds.Add("Cherven");
            landIds.Add("Cherykau");
            landIds.Add("Dobrush");
            landIds.Add("Dokshytsy");
            landIds.Add("Drahichyn");
            landIds.Add("Drybin");
            landIds.Add("Dubrouna");
            landIds.Add("Dzyarzhynsk");
            landIds.Add("Dzyatlava");
            landIds.Add("Glubokae");
            landIds.Add("Hantsavichy");
            landIds.Add("Haradok");
            landIds.Add("Hlusk");
            landIds.Add("Homel");
            landIds.Add("Horki");
            landIds.Add("Hrodna");
            landIds.Add("Iŭe");
            landIds.Add("Ivanava");
            landIds.Add("Ivatsevichy");
            landIds.Add("Kalinkavichy");
            landIds.Add("Kamyanec");
            landIds.Add("Kapyl");
            landIds.Add("Karelichy");
            landIds.Add("Karma");
            landIds.Add("Kastsukovichy");
            landIds.Add("Khoiniki");
            landIds.Add("Khotimsk");
            landIds.Add("Kirausk");
            landIds.Add("Kletsk");
            landIds.Add("Klichau");
            landIds.Add("Klimavichy");
            landIds.Add("Kobryn");
            landIds.Add("Krasnapolle");
            landIds.Add("Kruhlae");
            landIds.Add("Krupki");
            landIds.Add("Krychau");
            landIds.Add("Lahoisk");
            landIds.Add("Lelchytsy");
            landIds.Add("Lepel");
            landIds.Add("Lida");
            landIds.Add("Liuban");
            landIds.Add("Loeu");
            landIds.Add("Luninec");
            landIds.Add("Lyahavichy");
            landIds.Add("Lyozna");
            landIds.Add("Mahilou");
            landIds.Add("Maladzechna");
            landIds.Add("Malaryta");
            landIds.Add("Masty");
            landIds.Add("Mazyr");
            landIds.Add("Minsk");
            landIds.Add("Miyory");
            landIds.Add("Mstislaul");
            landIds.Add("Myadzel");
            landIds.Add("Naroulya");
            landIds.Add("Navahrudak");
            landIds.Add("Nyasvizh");
            landIds.Add("Orsha");
            landIds.Add("Pastavy");
            landIds.Add("Petrykau");
            landIds.Add("Pinsk");
            landIds.Add("Polatsk");
            landIds.Add("Pruzhany");
            landIds.Add("Puhavichy");
            landIds.Add("Rahachou");
            landIds.Add("Rasony");
            landIds.Add("Rechytsa");
            landIds.Add("Salihorsk");
            landIds.Add("Sharkaushchyna");
            landIds.Add("Shchuchin");
            landIds.Add("Shklou");
            landIds.Add("Shumilina");
            landIds.Add("Slauharad");
            landIds.Add("Slonim");
            landIds.Add("Slutsk");
            landIds.Add("Smalyavichy");
            landIds.Add("Smargon");
            landIds.Add("Staryja Darohi");
            landIds.Add("Staubcy");
            landIds.Add("Stolin");
            landIds.Add("Svetlahorsk");
            landIds.Add("Svislach");
            landIds.Add("Syanno");
            landIds.Add("Talachyn");
            landIds.Add("Ushachi");
            landIds.Add("Uzda");
            landIds.Add("Valozhyn");
            landIds.Add("Vaŭkavysk");
            landIds.Add("Verhnyadzvinsk");
            landIds.Add("Vetka");
            landIds.Add("Vileika");
            landIds.Add("Vitebsk");
            landIds.Add("Voranava");
            landIds.Add("Yelsk");
            landIds.Add("Zelva");
            landIds.Add("Zhabinka");
            landIds.Add("Zhlobin");
            landIds.Add("Zhytkavichy");

            landIds.Add("Ershichi");
            landIds.Add("Shumyachi");
            landIds.Add("Roslavl");
            landIds.Add("Hislavichi");
            landIds.Add("Monastyrschina");
            landIds.Add("Krasny");
            landIds.Add("Rudnya");
            landIds.Add("Pochinok");
            landIds.Add("Smolensk");
            landIds.Add("Demidov");
            landIds.Add("Velizh");
            landIds.Add("Elnya");
            landIds.Add("Glinka");
            landIds.Add("Kardymovo");
            landIds.Add("Duhovschina");
            landIds.Add("Yarcevo");
            landIds.Add("Dorogobuzh");
            landIds.Add("Safonovo");
            landIds.Add("Holm-Zhirkovski");
            landIds.Add("Ugra");
            landIds.Add("Vyazma");
            landIds.Add("Tsyomkino");
            landIds.Add("Novodugino");
            landIds.Add("Sychyovka");
            landIds.Add("Gzhatsk");
            Random randNum = new Random();
            int aRandomPos = randNum.Next(landIds.Count);
            return landIds[aRandomPos];
        }
    }
}
