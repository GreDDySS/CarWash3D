using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash3D.Data.Repositories
{
    public class EmployeeRepository
    {
        private readonly CarWashDbContext _context;

        public EmployeeRepository(CarWashDbContext context)
        {
            _context = context;
        }

        public Employee GetEmployeeByUsernameAndPassword(string username, string password)
        {
            return _context.Сотрудники.FirstOrDefault(e => e.Логин == username && e.Пароль == password);
        }
    }
}
