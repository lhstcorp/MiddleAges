﻿using Microsoft.EntityFrameworkCore;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiddleAges.Models;
using MiddleAges.Data;

namespace MiddleAges.Models
{
    public static class CommonLogic
    {
        public const double LandsCount = 207;
        public const double BaseLandBuildingPrice = 10;
        public const double BasePeasantIncome = 0.01;
        public const double BaseGoldLimit = 1000;
        public const double WarfareImpactPerc = 2.00;
        public const int    PlayerOnlineMinutes = 60;
        public const double LandBuildingDestructionPercentage = 0.5;
        public const double AttributePointResetCostMultiplier = 3.5;
        public const string IndependentLandsCountryId = "CAF6F518-3730-4692-AB56-FC755E7FF957";
        public const string AdminId = "04805b01-4ec4-4e98-ba9b-1b08c7054026";
        public const string RebelStateName = "Rebel state of {0}";
        public const string RebelStateColor = "#000000";

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
                (int)LawType.AppointGovernor => "{0} became the governor of {1}",
                (int)LawType.TransferingMoney => "{0} solids from the state treasury were transferred to {1}",
                (int)LawType.ChangingBanner => "Country banner has been changed",
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
                (int)LawType.AppointGovernor => "AppointGovernor",
                (int)LawType.TransferingMoney => "TransferingMoney",
                (int)LawType.ChangingBanner => "ChangingBanner",
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

        public static double GetFortificationValue(double currentFortification, double maxFortification)
        {
            return currentFortification/maxFortification;
        }

        public static long GetRequiredExpByLvl(long lvl)
        {
            long requiredExp = Convert.ToInt32(Math.Floor(Math.Pow(1.4, lvl)));

            return requiredExp;
        }

        public static double CalculateAttributePointReset(int playerLvl)
        {
            return playerLvl * AttributePointResetCostMultiplier;
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

            landIds.Add("Salchininkai");
            landIds.Add("Varena");
            landIds.Add("Druskininkai");
            landIds.Add("Lazdijai");
            landIds.Add("Alytus");
            landIds.Add("Trakai");
            landIds.Add("Vilnius");
            landIds.Add("Svencionys");
            landIds.Add("Ignalina");
            landIds.Add("Zarasai");
            landIds.Add("Kalvarija");
            landIds.Add("Marijampole");
            landIds.Add("Prienai");
            landIds.Add("Elektrenai");
            landIds.Add("Kaisiadorys");
            landIds.Add("Sirvintos");
            landIds.Add("Moletai");
            landIds.Add("Utena");
            landIds.Add("Rokiskis");
            landIds.Add("Kupiskis");
            landIds.Add("Anyksciai");
            landIds.Add("Ukmerge");
            landIds.Add("Ionava");
            landIds.Add("Kaunas");
            landIds.Add("Kazlu Ruda");
            landIds.Add("Vilaviskis");
            landIds.Add("Sakiai");
            landIds.Add("Jurbarkas");
            landIds.Add("Raseiniai");
            landIds.Add("Kedainiai");
            landIds.Add("Panevezys");
            landIds.Add("Birzai");
            landIds.Add("Pasvalys");
            landIds.Add("Radviliskis");
            landIds.Add("Pakruojis");
            landIds.Add("Joniskis");
            landIds.Add("Siauliai");
            landIds.Add("Kelme");
            landIds.Add("Silale");
            landIds.Add("Taurage");
            landIds.Add("Pagegiai");
            landIds.Add("Akmene");
            landIds.Add("Mazeikiai");
            landIds.Add("Telsiai");
            landIds.Add("Rietavas");
            landIds.Add("Plunge");
            landIds.Add("Skuodas");
            landIds.Add("Kretinga");
            landIds.Add("Klaipeda");
            landIds.Add("Silute");

            landIds.Add("Hajnowka");
            landIds.Add("Siemiatycze");
            landIds.Add("Bielsk Podlaski");
            landIds.Add("Bialystok");
            landIds.Add("Sokolka");
            landIds.Add("Augustow");
            landIds.Add("Sejny");
            landIds.Add("Suwalki");
            landIds.Add("Wysokie Mazowieckie");
            landIds.Add("Monki");
            landIds.Add("Zambrow");
            landIds.Add("Lomza");
            landIds.Add("Grajewo");
            landIds.Add("Kolno");

            landIds.Add("Kovel");
            landIds.Add("Kamen-Kashirski");
            landIds.Add("Volodimir");
            landIds.Add("Lutsk");
            landIds.Add("Varash");
            landIds.Add("Sarni");
            landIds.Add("Rivne");
            landIds.Add("Dubno");
            landIds.Add("Zvyagel");
            landIds.Add("Korosten");
            landIds.Add("Zhitomir");
            landIds.Add("Berdichiv");
            landIds.Add("Vishgorod");
            landIds.Add("Bucha");
            landIds.Add("Fastiv");
            landIds.Add("Kyiv");
            landIds.Add("Brovari");
            landIds.Add("Borispil");
            landIds.Add("Obuhiv");
            landIds.Add("Bila Cerkva");
            landIds.Add("Nizhin");
            landIds.Add("Chernigiv");
            landIds.Add("Korukivka");
            landIds.Add("Novgorod-Siverskii");
            landIds.Add("Priluki");

            landIds.Add("Kraslava");
            landIds.Add("Daugavpils");
            landIds.Add("Ludza");
            landIds.Add("Rezekne");
            landIds.Add("Preili");
            landIds.Add("Livani");
            landIds.Add("Balvi");
            landIds.Add("Aluksne");
            landIds.Add("Gulbene");
            landIds.Add("Varaklani");
            landIds.Add("Madona");
            landIds.Add("Jekabpils");
            landIds.Add("Aizkraukle");
            landIds.Add("Bauska");
            landIds.Add("Smiltene");
            landIds.Add("Valka");
            landIds.Add("Valmiera");
            landIds.Add("Cesis");
            landIds.Add("Ogre");
            landIds.Add("Limbazi");
            landIds.Add("Sigulda");
            landIds.Add("Saulkrasti");
            landIds.Add("Adazi");
            landIds.Add("Ulbroka");
            landIds.Add("Salaspils");
            landIds.Add("Kekava");
            landIds.Add("Riga");
            landIds.Add("Olaine");
            landIds.Add("Marupe");
            landIds.Add("Jurmala");
            landIds.Add("Jelgava");
            landIds.Add("Dobele");
            landIds.Add("Saldus");
            landIds.Add("Tukums");
            landIds.Add("Liepaja");
            landIds.Add("Kuldiga");
            landIds.Add("Talsi");
            landIds.Add("Ventspils");

            Random randNum = new Random();
            int aRandomPos = randNum.Next(landIds.Count);
            return landIds[aRandomPos];
        }

        public static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static void UpdateLastPlayerActivityDateTime(ApplicationDbContext context, Player player)
        {
            player.LastActivityDateTime = DateTime.UtcNow;
            context.Update(player);

            context.SaveChanges();
        }
    }
}
