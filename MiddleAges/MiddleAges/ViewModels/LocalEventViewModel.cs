using MiddleAges.Temporary_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MiddleAges.HelperClasses.LocalEventHelper;

namespace MiddleAges.ViewModels
{
    public class LocalEventViewModel
    {
        public LocalEvent LocalEvent { get; set; }
        public DateTime AssignedDateTime { get; set; }
        public string Option1Element { get; set; }
        public string Option2Element { get; set; }
    }
}
