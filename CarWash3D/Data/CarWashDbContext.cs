using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash3D.Data
{
    public class CarWashDbContext : DbContext
    {
        public DbSet<Client> Клиенты { get; set; }
        public DbSet<Employee> Сотрудники { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Data/CarWash.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Клиенты");
            modelBuilder.Entity<Client>().HasKey(c => c.ID_Клиента); 
            modelBuilder.Entity<Client>().Property(c => c.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<Client>().Property(c => c.Фамилия).HasColumnName("Фамилия");
            modelBuilder.Entity<Client>().Property(c => c.Имя).HasColumnName("Имя");
            modelBuilder.Entity<Client>().Property(c => c.Отчество).HasColumnName("Отчество");
            modelBuilder.Entity<Client>().Property(c => c.Номер_телефона).HasColumnName("Номер_телефона");
            modelBuilder.Entity<Client>().Property(c => c.Номер_машины).HasColumnName("Номер_машины");

            modelBuilder.Entity<Employee>().ToTable("Сотрудники");
            modelBuilder.Entity<Employee>().HasKey(e => e.ID_Сотрудника); 
            modelBuilder.Entity<Employee>().Property(e => e.ID_Сотрудника).HasColumnName("ID_Сотрудника");
            modelBuilder.Entity<Employee>().Property(e => e.Фамилия).HasColumnName("Фамилия");
            modelBuilder.Entity<Employee>().Property(e => e.Имя).HasColumnName("Имя");
            modelBuilder.Entity<Employee>().Property(e => e.Отчество).HasColumnName("Отчество");
            modelBuilder.Entity<Employee>().Property(e => e.Номер_телефона).HasColumnName("Номер_телефона");
            modelBuilder.Entity<Employee>().Property(e => e.ID_Должность).HasColumnName("ID_Должность");
            modelBuilder.Entity<Employee>().Property(e => e.Пароль).HasColumnName("Пароль");
            modelBuilder.Entity<Employee>().Property(e => e.Логин).HasColumnName("Логин");
        }
    }
}
