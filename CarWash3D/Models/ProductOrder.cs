using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class ProductOrder
    {
        public int ID_Заказ_Товаров { get; set; }
        public int ID_Товара { get; set; }
        public Product Product { get; set; }
        public int Кол_во { get; set; }
        public int ID_Клиента { get; set; }
        public Client Client { get; set; }
        public int ID_Сотрудника { get; set; }
        public Employee Employee { get; set; }
        public int ID_Статуса { get; set; }
        public Status Status { get; set; }
        public DateTime Дата_и_время { get; set; }
    }
}
