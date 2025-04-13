using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Service
    {
        public int ID_Услуги { get; set; }
        public string Наименование { get; set; }
        public int Стоимость { get; set; }
        public int Длительность { get; set; }
        public string Описание { get; set; }
        public int? ID_Акции { get; set; }
        public Promotion Promotion { get; set; }
    }
}
