using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace CarWash3D.Data.Repositories
{
    public class ClientRepository
    {
        private readonly CarWashDbContext _context;

        public ClientRepository(CarWashDbContext context)
        {
            _context = context;
        }

        public Client GetClientByPhoneAndCarNumber(string phone, string carNumber)
        {
            return _context.Клиенты.FirstOrDefault(c => c.Номер_телефона == phone && c.Номер_машины == carNumber);
        }

        public bool AddClient(Client client)
        {
            try
            {
                _context.Клиенты.Add(client);
                int result = _context.SaveChanges();
                return result > 0; 
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
