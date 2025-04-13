using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Promotion
    {
        public int ID_Акции { get; set; }
        public string Наименование { get; set; }
        public int Скидка { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
