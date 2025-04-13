using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Product
    {
        public int ID_Товара { get; set; }
        public string Наименование { get; set; }
        public int Стоимость { get; set; }
        public int Кол_во { get; set; }
        public string Описание { get; set; }
        public string Фотография { get; set; }
        public int ID_Категории { get; set; }
        public Category Category { get; set; }
        public int? ID_Акции { get; set; }
        public Promotion Promotion { get; set; }
    }
}
