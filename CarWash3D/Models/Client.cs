using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Client
    {
        public int ID_Клиента { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string Номер_телефона { get; set; }
        public string Номер_машины { get; set; }
    }
}
