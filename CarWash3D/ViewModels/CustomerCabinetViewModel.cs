using CarWash3D.Data;
using CarWash3D.Helpers;
using CarWash3D.Services;
using CarWash3D.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class CustomerCabinetViewModel : INotifyPropertyChanged
    {
        private readonly IDbContextFactory<CarWashDbContext> _dbContextFactory;
        private readonly CarWashDbContext _dbContext;
        private readonly ClientService _clientService;
        private readonly NavigationService _navigationService;
        private int _clientId;

        public int BonusBalance { get; set; }
        public string LastVisit { get; set; }
        public string NextVisit { get; set; }
        public int TotalVisits { get; set; }

        public ObservableCollection<Booking> UpcomingBookings { get; } = new ObservableCollection<Booking>();
        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        public ICommand LogoutCommand { get; }
        public ICommand ShowProfileCommand { get; }
        public ICommand ShowBonusDetailsCommand { get; }
        public ICommand ShowVisitHistoryCommand { get; }
        public ICommand ShowCatalogCommand { get; }
        public ICommand BookServiceCommand { get; }
        public ICommand ShowOrderDetailsCommand { get; }

        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                LoadClientData(); // Перезагружаем данные при изменении ClientId
                OnPropertyChanged(nameof(ClientId));
            }
        }

        public CustomerCabinetViewModel(IDbContextFactory<CarWashDbContext> dbContextFactory, ClientService clientService, NavigationService navigationService, int clientId)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
            _clientService = clientService;
            _navigationService = navigationService;
            _clientId = clientId;

            LoadClientData();

            LogoutCommand = new RelayCommand(_ => Logout());
            ShowProfileCommand = new RelayCommand(_ => ShowProfile());
            ShowBonusDetailsCommand = new RelayCommand(_ => ShowBonusDetails());
            ShowVisitHistoryCommand = new RelayCommand(_ => ShowVisitHistory());
            ShowCatalogCommand = new RelayCommand(_ => ShowCatalog());
            BookServiceCommand = new RelayCommand(_ => BookService());
            ShowOrderDetailsCommand = new RelayCommand(ShowOrderDetails);
        }

        private void LoadClientData()
        {
            var client = _dbContext.Клиенты
                .Include(c => c.VisitHistories)
                .Include(c => c.ServiceOrders)
                .ThenInclude(so => so.Service)
                .Include(c => c.ProductOrders)
                .ThenInclude(po => po.Product)
                .FirstOrDefault(c => c.ID_Клиента == _clientId);

            if (client != null)
            {
                var bonus = _dbContext.Бонусы
                    .Where(b => b.ID_Клиента == _clientId)
                    .OrderByDescending(b => b.ID_Бонуса)
                    .FirstOrDefault();
                BonusBalance = bonus?.Кол_во ?? 0;

                var lastVisit = client.VisitHistories.OrderByDescending(v => v.Дата_и_время).FirstOrDefault();
                LastVisit = lastVisit != null ? lastVisit.Дата_и_время.ToString("dd.MM.yyyy HH:mm") : "Нет посещений";

                var nextBooking = client.ServiceOrders
                    .Where(so => so.Дата_и_время > DateTime.Now && so.Status.Название == "Ожидает")
                    .OrderBy(so => so.Дата_и_время)
                    .FirstOrDefault();
                NextVisit = nextBooking != null ? nextBooking.Дата_и_время.ToString("dd.MM.yyyy HH:mm") : "Нет записей";

                TotalVisits = client.VisitHistories.Count;

                LoadUpcomingBookings();

                // Загружаем заказы
                var orders = _dbContext.Заказы_Товаров
                    .Include(po => po.Product)
                    .Include(po => po.Status)
                    .Where(po => po.ID_Клиента == _clientId)
                    .GroupBy(po => new { po.ID_Заказ_Товаров, po.Дата_и_время, po.ID_Статуса })
                    .Select(g => new Order
                    {
                        OrderId = g.Key.ID_Заказ_Товаров,
                        ClientId = _clientId,
                        Details = $"{g.Key.Дата_и_время:dd.MM.yyyy} • {g.Count()} товара",
                        Status = g.First().Status.Название
                    })
                    .ToList();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
        }

        private void LoadUpcomingBookings()
        {
            UpcomingBookings.Clear();
            var upcomingBookings = _dbContext.Заказы_Услуг
                .Include(so => so.Service)
                .Include(so => so.Status)
                .Where(so => so.ID_Клиента == _clientId && so.Дата_и_время > DateTime.Now && so.Status.Название == "Ожидает")
                .Select(so => new Booking
                {
                    ServiceName = so.Service.Наименование,
                    DateTime = so.Дата_и_время.ToString("dd.MM.yyyy HH:mm"),
                    Status = so.Status.Название
                })
                .ToList();
            foreach (var booking in upcomingBookings)
            {
                UpcomingBookings.Add(booking);
            }

            // Обновляем NextVisit после загрузки записей
            var nextBooking = _dbContext.Заказы_Услуг
                .Include(so => so.Status)
                .Where(so => so.ID_Клиента == _clientId && so.Дата_и_время > DateTime.Now && so.Status.Название == "Ожидает")
                .OrderBy(so => so.Дата_и_время)
                .FirstOrDefault();
            NextVisit = nextBooking != null ? nextBooking.Дата_и_время.ToString("dd.MM.yyyy HH:mm") : "Нет записей";
            OnPropertyChanged(nameof(NextVisit));
        }

        private void Logout()
        {
            _navigationService.NavigateTo("LoginView");
            MessageBox.Show("Вы вышли из системы.");
        }

        private void ShowProfile()
        {
            var viewModel = new ProfileViewModel(_dbContextFactory, _clientId);
            var window = new ProfileWindow(viewModel);
            viewModel.SetWindow(window);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ShowBonusDetails() => MessageBox.Show("Подробности о бонусах (в разработке)");

        private void ShowVisitHistory()
        {
            var viewModel = new VisitHistoryViewModel(_dbContextFactory, _clientId);
            var window = new VisitHistoryWindow(viewModel);
            viewModel.SetWindow(window);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ShowCatalog() => MessageBox.Show("Каталог товаров (в разработке)");

        private void BookService()
        {
            var viewModel = new BookServiceViewModel(_dbContextFactory, _clientId);
            var window = new BookServiceWindow(viewModel);
            viewModel.SetWindow(window);

            viewModel.BookingAdded += (sender, serviceOrder) =>
            {
                using (var tempDbContext = _dbContextFactory.CreateDbContext())
                {
                    var addedOrder = tempDbContext.Заказы_Услуг
                        .Include(so => so.Service)
                        .Include(so => so.Status)
                        .FirstOrDefault(so => so.ID_Клиента == _clientId && so.Дата_и_время == serviceOrder.Дата_и_время);

                    if (addedOrder != null && addedOrder.Status?.Название == "Ожидает" && addedOrder.Дата_и_время > DateTime.Now)
                    {
                        var newBooking = new Booking
                        {
                            ServiceName = addedOrder.Service?.Наименование ?? "Неизвестная услуга",
                            DateTime = addedOrder.Дата_и_время.ToString("dd.MM.yyyy HH:mm"),
                            Status = addedOrder.Status?.Название ?? "Неизвестный статус"
                        };
                        UpcomingBookings.Add(newBooking);

                        var nextBooking = UpcomingBookings
                            .Select(b => DateTime.ParseExact(b.DateTime, "dd.MM.yyyy HH:mm", null))
                            .Where(dt => dt > DateTime.Now)
                            .OrderBy(dt => dt)
                            .FirstOrDefault();
                        NextVisit = nextBooking != default ? nextBooking.ToString("dd.MM.yyyy HH:mm") : "Нет записей";
                        OnPropertyChanged(nameof(NextVisit));
                    }
                }
            };

            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        private void ShowOrderDetails(object parameter)
        {
            if (parameter is Order order)
            {
                var viewModel = new OrderDetailsViewModel(_dbContextFactory, order);
                var window = new OrderDetailsWindow(viewModel);
                viewModel.SetWindow(window);
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Booking
    {
        public string ServiceName { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
    }
}