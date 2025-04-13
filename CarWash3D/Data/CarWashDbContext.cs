using CarWash3D.Models;
using CarWash3D.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarWash3D.Data
{
    public class CarWashDbContext : DbContext
    {
        public DbSet<Client> Клиенты { get; set; }
        public DbSet<Employee> Сотрудники { get; set; }
        public DbSet<Review> Отзывы { get; set; }
        public DbSet<Bonus> Бонусы { get; set; }
        public DbSet<BonusHistory> История_Бонусов { get; set; }
        public DbSet<Promotion> Акции { get; set; }
        public DbSet<VisitHistory> История_Посещений { get; set; }
        public DbSet<Models.Service> Услуги { get; set; }
        public DbSet<ServiceOrder> Заказы_Услуг { get; set; }
        public DbSet<Status> Статусы { get; set; }
        public DbSet<Position> Должности { get; set; }
        public DbSet<Product> Товары { get; set; }
        public DbSet<ProductOrder> Заказы_Товаров { get; set; }
        public DbSet<Category> Категории { get; set; }

        public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Клиенты
            modelBuilder.Entity<Client>().ToTable("Клиенты");
            modelBuilder.Entity<Client>().HasKey(c => c.ID_Клиента);
            modelBuilder.Entity<Client>().Property(c => c.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<Client>().Property(c => c.Фамилия).HasColumnName("Фамилия");
            modelBuilder.Entity<Client>().Property(c => c.Имя).HasColumnName("Имя");
            modelBuilder.Entity<Client>().Property(c => c.Отчество).HasColumnName("Отчество");
            modelBuilder.Entity<Client>().Property(c => c.Номер_телефона).HasColumnName("Номер_телефона");
            modelBuilder.Entity<Client>().Property(c => c.Номер_машины).HasColumnName("Номер_машины");

            // Сотрудники
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

            // Отзывы
            modelBuilder.Entity<Review>().ToTable("Отзывы");
            modelBuilder.Entity<Review>().HasKey(r => r.ID_Отзыва);
            modelBuilder.Entity<Review>().Property(r => r.ID_Отзыва).HasColumnName("ID_Отзыва");
            modelBuilder.Entity<Review>().Property(r => r.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<Review>().Property(r => r.Отзыв).HasColumnName("Отзыв");
            modelBuilder.Entity<Review>().Property(r => r.Оценка).HasColumnName("Оценка");
            modelBuilder.Entity<Review>().Property(r => r.Дата_и_время).HasColumnName("Дата_и_время");

            // Бонусы
            modelBuilder.Entity<Bonus>().ToTable("Бонусы");
            modelBuilder.Entity<Bonus>().HasKey(b => b.ID_Бонуса);
            modelBuilder.Entity<Bonus>().Property(b => b.ID_Бонуса).HasColumnName("ID_Бонуса");
            modelBuilder.Entity<Bonus>().Property(b => b.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<Bonus>().Property(b => b.Кол_во).HasColumnName("Кол_во");

            // История_Бонусов
            modelBuilder.Entity<BonusHistory>().ToTable("История_Бонусов");
            modelBuilder.Entity<BonusHistory>().HasKey(bh => bh.ID_Ист_Бонусов);
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.ID_Ист_Бонусов).HasColumnName("ID_Ист_Бонусов");
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.ID_Бонус).HasColumnName("ID_Бонус");
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.Наименование).HasColumnName("Наименование");
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.Кол_во).HasColumnName("Кол_во");
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.Тип).HasColumnName("Тип");
            modelBuilder.Entity<BonusHistory>().Property(bh => bh.Дата_и_время).HasColumnName("Дата_и_время");

            // Акции
            modelBuilder.Entity<Promotion>().ToTable("Акции");
            modelBuilder.Entity<Promotion>().HasKey(p => p.ID_Акции);
            modelBuilder.Entity<Promotion>().Property(p => p.ID_Акции).HasColumnName("ID_Акции");
            modelBuilder.Entity<Promotion>().Property(p => p.Наименование).HasColumnName("Наименование");
            modelBuilder.Entity<Promotion>().Property(p => p.Скидка).HasColumnName("Скидка");
            modelBuilder.Entity<Promotion>().Property(p => p.Дата_и_время).HasColumnName("Дата_и_время");

            // История_Посещений
            modelBuilder.Entity<VisitHistory>().ToTable("История_Посещений");
            modelBuilder.Entity<VisitHistory>().HasKey(vh => vh.ID_Ист_Посещений);
            modelBuilder.Entity<VisitHistory>().Property(vh => vh.ID_Ист_Посещений).HasColumnName("ID_Ист_Посещений");
            modelBuilder.Entity<VisitHistory>().Property(vh => vh.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<VisitHistory>().Property(vh => vh.Дата_и_время).HasColumnName("Дата_и_время");

            // Услуги
            modelBuilder.Entity<Models.Service>().ToTable("Услуги");
            modelBuilder.Entity<Models.Service>().HasKey(s => s.ID_Услуги);
            modelBuilder.Entity<Models.Service>().Property(s => s.ID_Услуги).HasColumnName("ID_Услуги");
            modelBuilder.Entity<Models.Service>().Property(s => s.Наименование).HasColumnName("Наименование");
            modelBuilder.Entity<Models.Service>().Property(s => s.Стоимость).HasColumnName("Стоимость");
            modelBuilder.Entity<Models.Service>().Property(s => s.Длительность).HasColumnName("Длительность");
            modelBuilder.Entity<Models.Service>().Property(s => s.Описание).HasColumnName("Описание");
            modelBuilder.Entity<Models.Service>().Property(s => s.ID_Акции).HasColumnName("ID_Акции");

            // Заказы_Услуг
            modelBuilder.Entity<ServiceOrder>().ToTable("Заказы_Услуг");
            modelBuilder.Entity<ServiceOrder>().HasKey(so => so.ID_Заказ_Услуг);
            modelBuilder.Entity<ServiceOrder>().Property(so => so.ID_Заказ_Услуг).HasColumnName("ID_Заказ_Услуг");
            modelBuilder.Entity<ServiceOrder>().Property(so => so.ID_Услуги).HasColumnName("ID_Услуги");
            modelBuilder.Entity<ServiceOrder>().Property(so => so.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<ServiceOrder>().Property(so => so.ID_Сотрудника).HasColumnName("ID_Сотрудника");
            modelBuilder.Entity<ServiceOrder>().Property(so => so.ID_Статуса).HasColumnName("ID_Статуса");
            modelBuilder.Entity<ServiceOrder>().Property(so => so.Дата_и_время).HasColumnName("Дата_и_время");

            // Статусы
            modelBuilder.Entity<Status>().ToTable("Статусы");
            modelBuilder.Entity<Status>().HasKey(s => s.ID_Статуса);
            modelBuilder.Entity<Status>().Property(s => s.ID_Статуса).HasColumnName("ID_Статуса");
            modelBuilder.Entity<Status>().Property(s => s.Название).HasColumnName("Название");

            // Должности
            modelBuilder.Entity<Position>().ToTable("Должности");
            modelBuilder.Entity<Position>().HasKey(p => p.ID_Должность);
            modelBuilder.Entity<Position>().Property(p => p.ID_Должность).HasColumnName("ID_Должность");
            modelBuilder.Entity<Position>().Property(p => p.Наименование).HasColumnName("Наименование");
            modelBuilder.Entity<Position>().Property(p => p.Зарплата).HasColumnName("Зарплата");

            // Товары
            modelBuilder.Entity<Product>().ToTable("Товары");
            modelBuilder.Entity<Product>().HasKey(p => p.ID_Товара);
            modelBuilder.Entity<Product>().Property(p => p.ID_Товара).HasColumnName("ID_Товара");
            modelBuilder.Entity<Product>().Property(p => p.Наименование).HasColumnName("Наименование");
            modelBuilder.Entity<Product>().Property(p => p.Стоимость).HasColumnName("Стоимость");
            modelBuilder.Entity<Product>().Property(p => p.Кол_во).HasColumnName("Кол_во");
            modelBuilder.Entity<Product>().Property(p => p.Описание).HasColumnName("Описание");
            modelBuilder.Entity<Product>().Property(p => p.Фотография).HasColumnName("Фотография");
            modelBuilder.Entity<Product>().Property(p => p.ID_Категории).HasColumnName("ID_Категории");
            modelBuilder.Entity<Product>().Property(p => p.ID_Акции).HasColumnName("ID_Акции");

            // Заказы_Товаров
            modelBuilder.Entity<ProductOrder>().ToTable("Заказы_Товаров");
            modelBuilder.Entity<ProductOrder>().HasKey(po => po.ID_Заказ_Товаров);
            modelBuilder.Entity<ProductOrder>().Property(po => po.ID_Заказ_Товаров).HasColumnName("ID_Заказ_Товаров");
            modelBuilder.Entity<ProductOrder>().Property(po => po.ID_Товара).HasColumnName("ID_Товара");
            modelBuilder.Entity<ProductOrder>().Property(po => po.Кол_во).HasColumnName("Кол_во");
            modelBuilder.Entity<ProductOrder>().Property(po => po.ID_Клиента).HasColumnName("ID_Клиента");
            modelBuilder.Entity<ProductOrder>().Property(po => po.ID_Сотрудника).HasColumnName("ID_Сотрудника");
            modelBuilder.Entity<ProductOrder>().Property(po => po.ID_Статуса).HasColumnName("ID_Статуса");
            modelBuilder.Entity<ProductOrder>().Property(po => po.Дата_и_время).HasColumnName("Дата_и_время");

            // Категории
            modelBuilder.Entity<Category>().ToTable("Категории");
            modelBuilder.Entity<Category>().HasKey(c => c.ID_Категории);
            modelBuilder.Entity<Category>().Property(c => c.ID_Категории).HasColumnName("ID_Категории");
            modelBuilder.Entity<Category>().Property(c => c.Наименование).HasColumnName("Наименование");

            // Настройка связей
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.ID_Клиента);

            modelBuilder.Entity<Bonus>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bonuses)
                .HasForeignKey(b => b.ID_Клиента);

            modelBuilder.Entity<BonusHistory>()
                .HasOne(bh => bh.Bonus)
                .WithMany(b => b.BonusHistories)
                .HasForeignKey(bh => bh.ID_Бонус);

            modelBuilder.Entity<VisitHistory>()
                .HasOne(vh => vh.Client)
                .WithMany(c => c.VisitHistories)
                .HasForeignKey(vh => vh.ID_Клиента);

            modelBuilder.Entity<Models.Service>()
                .HasOne(s => s.Promotion)
                .WithMany()
                .HasForeignKey(s => s.ID_Акции);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Service)
                .WithMany()
                .HasForeignKey(so => so.ID_Услуги);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Client)
                .WithMany(c => c.ServiceOrders)
                .HasForeignKey(so => so.ID_Клиента);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Employee)
                .WithMany()
                .HasForeignKey(so => so.ID_Сотрудника);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Status)
                .WithMany()
                .HasForeignKey(so => so.ID_Статуса);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.position)
                .WithMany()
                .HasForeignKey(e => e.ID_Должность);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.ID_Категории);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Promotion)
                .WithMany()
                .HasForeignKey(p => p.ID_Акции);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Product)
                .WithMany()
                .HasForeignKey(po => po.ID_Товара);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Client)
                .WithMany(c => c.ProductOrders)
                .HasForeignKey(po => po.ID_Клиента);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Employee)
                .WithMany()
                .HasForeignKey(po => po.ID_Сотрудника);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(po => po.Status)
                .WithMany()
                .HasForeignKey(po => po.ID_Статуса);
        }
    }
}
