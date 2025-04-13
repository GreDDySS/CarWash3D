using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class BonusHistory
    {
        public int ID_Ист_Бонусов { get; set; }
        public int ID_Бонус { get; set; }
        public Bonus Bonus { get; set; }
        public string Наименование { get; set; }
        public int Кол_во { get; set; }
        public string Тип { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
