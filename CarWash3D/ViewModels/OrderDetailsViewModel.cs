using CarWash3D.Data;
using Microsoft.EntityFrameworkCore;
using CarWash3D.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CarWash3D.Models;

namespace CarWash3D.ViewModels
{
   
    public class OrderDetailsViewModel : INotifyPropertyChanged
    {
        private readonly CarWashDbContext _dbContext;
        private Window _window;

        public string OrderTitle { get; set; }
        public string OrderDate { get; set; }
        public ObservableCollection<OrderItem> OrderItems { get; } = new ObservableCollection<OrderItem>();
        public string OrderTotal { get; set; }
        public string UsedBonuses { get; set; }
        public string FinalTotal { get; set; }
        public string AccruedBonuses { get; set; }

        public ICommand CloseCommand { get; }

        public OrderDetailsViewModel(IDbContextFactory<CarWashDbContext> dbContextFactory, Order order)
        {
            _dbContext = dbContextFactory.CreateDbContext();

            // Загружаем детали заказа
            var dbOrder = _dbContext.Заказы_Товаров
                .Include(o => o.Product)
                .Where(o => o.ID_Заказ_Товаров == order.OrderId)
                .GroupBy(o => new { o.ID_Заказ_Товаров, o.Дата_и_время })
                .Select(g => new
                {
                    g.Key.ID_Заказ_Товаров,
                    g.Key.Дата_и_время,
                    Items = g.ToList()
                })
                .FirstOrDefault();

            if (dbOrder != null)
            {
                OrderTitle = $"Детали заказа #{dbOrder.ID_Заказ_Товаров}";
                OrderDate = dbOrder.Дата_и_время.ToString("dd.MM.yyyy");

                foreach (var item in dbOrder.Items)
                {
                    OrderItems.Add(new OrderItem
                    {
                        Name = item.Product.Наименование,
                        Quantity = item.Кол_во,
                        Price = item.Product.Стоимость,
                        TotalPrice = $"{item.Кол_во * item.Product.Стоимость} ₽"
                    });
                }

                var total = dbOrder.Items.Sum(i => i.Кол_во * i.Product.Стоимость);
                OrderTotal = $"{total} ₽";

                // Бонусы
                var bonus = _dbContext.Бонусы
                .Where(b => b.ID_Клиента == order.ClientId)
                .OrderByDescending(b => b.ID_Бонуса)
                .FirstOrDefault();

                BonusHistory usedBonus = null;
                if (bonus != null)
                {
                    usedBonus = _dbContext.История_Бонусов
                        .Where(bh => bh.ID_Бонус == bonus.ID_Бонуса && bh.Тип == "Списание")
                        .OrderByDescending(bh => bh.Дата_и_время)
                        .FirstOrDefault();
                }

                UsedBonuses = usedBonus != null ? $"-{usedBonus.Кол_во} ₽" : "0 ₽";
                int usedAmount = usedBonus?.Кол_во ?? 0;
                FinalTotal = $"{total - usedAmount} ₽";

                // Начисление бонусов
                int bonusAmount = (int)(total * 0.05); // 5% от суммы заказа
                if (bonus != null)
                {
                    bonus.Кол_во += bonusAmount;
                }
                else
                {
                    bonus = new Bonus
                    {
                        ID_Клиента = order.ClientId,
                        Кол_во = bonusAmount
                    };
                    _dbContext.Бонусы.Add(bonus);
                }

                var bonusHistory = new BonusHistory
                {
                    ID_Бонус = bonus.ID_Бонуса,
                    Наименование = $"Начисление за заказ #{dbOrder.ID_Заказ_Товаров}",
                    Кол_во = bonusAmount,
                    Тип = "Начисление",
                    Дата_и_время = DateTime.Now
                };
                _dbContext.История_Бонусов.Add(bonusHistory);

                _dbContext.SaveChanges();

                AccruedBonuses = $"За этот заказ вам начислено {bonusAmount} бонусов (5% от суммы заказа)";
            }

            CloseCommand = new RelayCommand(_ => _window?.Close());
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string TotalPrice { get; set; }
        public string QuantityText => $"Количество: {Quantity} | Итого: {Price * Quantity} ₽";
    }
}