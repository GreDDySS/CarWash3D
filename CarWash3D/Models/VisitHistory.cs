using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class VisitHistory
    {
        public int ID_Ист_Посещений { get; set; }
        public int ID_Клиента { get; set; }
        public Client Client { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
