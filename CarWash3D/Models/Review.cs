using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Review
    {
        public int ID_Отзыва { get; set; }
        public int ID_Клиента { get; set; }
        public Client Client { get; set; }
        public string Отзыв { get; set; }
        public int Оценка { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
