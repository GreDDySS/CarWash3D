using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class ServiceOrder
    {
        public int ID_Заказ_Услуг { get; set; }
        public int ID_Услуги { get; set; }
        public Service Service { get; set; }
        public int ID_Клиента { get; set; }
        public Client Client { get; set; }
        public int ID_Сотрудника { get; set; }
        public Employee Employee { get; set; }
        public int ID_Статуса { get; set; }
        public Status Status { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
