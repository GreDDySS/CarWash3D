using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Bonus
    {
        public int ID_Бонуса { get; set; }
        public int ID_Клиента { get; set; }
        public Client Client { get; set; }
        public int Кол_во { get; set; }
        public List<BonusHistory> BonusHistories { get; set; }
    }
}
