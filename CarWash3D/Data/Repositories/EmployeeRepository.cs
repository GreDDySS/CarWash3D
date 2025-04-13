using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash3D.Data.Repositories
{
    public class EmployeeRepository
    {
        private readonly IDbContextFactory<CarWashDbContext> _dbContextFactory;

        public EmployeeRepository(IDbContextFactory<CarWashDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Employee? GetEmployeeByUsernameAndPassword(string username, string password)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Сотрудники.FirstOrDefault(e => e.Логин == username && e.Пароль == password);
        }
    }
}
