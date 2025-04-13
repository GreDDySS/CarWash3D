using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash3D.Models
{
    public class Employee
    {
        public int ID_Сотрудника { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string Номер_телефона { get; set; }
        public int ID_Должность { get; set; }
        public Position position { get; set; }
        public string Пароль { get; set; }
        public string Логин { get; set; }
    }
}
