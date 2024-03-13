using Microsoft.EntityFrameworkCore;
using MiddleAges.Entities;
using MiddleAges.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //public static string getRecruitCountById(int _id)
        //{
        //    string recruitCount = "";

        //    switch (_id)
        //    {
        //        case 1:
        //            recruitCount = "5000";
        //            break;
        //        case 2:
        //            recruitCount = "456";
        //            break;
        //    }

        //    return recruitCount;
        //}

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

        public static int getBuildingPrice(int buildingType)
        {

           
            int buildingPrice = buildingType switch
            {
                (int)BuildingType.Estate => (int)BuildingPrice.Estate,
                (int)BuildingType.Barracks => (int)BuildingPrice.Barracks,
                _ => 0
            };


            return buildingPrice;
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
            

            Random randNum = new Random();
            int aRandomPos = randNum.Next(landIds.Count);

            return landIds[aRandomPos];
        }
    }
}
