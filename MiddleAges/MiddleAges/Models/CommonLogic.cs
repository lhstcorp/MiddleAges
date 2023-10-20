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

    }
}
