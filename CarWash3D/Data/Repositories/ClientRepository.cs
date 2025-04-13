using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace CarWash3D.Data.Repositories
{
    public class ClientRepository
    {
        private readonly IDbContextFactory<CarWashDbContext> _dbContextFactory;

        public ClientRepository(IDbContextFactory<CarWashDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Client GetClientByPhoneAndCarNumber(string phone, string carNumber)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Клиенты.FirstOrDefault(c => c.Номер_телефона == phone && c.Номер_машины == carNumber);
        }

        public Client GetClientByPhone(string phone)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Клиенты.FirstOrDefault(c => c.Номер_телефона == phone);
        }

        public bool AddClient(Client client)
        {
            using var context = _dbContextFactory.CreateDbContext();
            try
            {
                context.Клиенты.Add(client);
                int result = context.SaveChanges();
                return result > 0; 
            }
            catch
            {
                return false;
            }
        }
    }
}
