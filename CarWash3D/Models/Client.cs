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
        public List<Review> Reviews { get; set; }
        public List<Bonus> Bonuses { get; set; }
        public List<VisitHistory> VisitHistories { get; set; }
        public List<ServiceOrder> ServiceOrders { get; set; }
        public List<ProductOrder> ProductOrders { get; set; }
    }
}
