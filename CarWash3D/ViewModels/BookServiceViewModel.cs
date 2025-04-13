using CarWash3D.Data;
using CarWash3D.Helpers;
using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class BookServiceViewModel : INotifyPropertyChanged
    {
        private readonly CarWashDbContext _dbContext;
        private readonly int _clientId;
        private Window _window;
        private Service _selectedService;
        private DateTime? _selectedDate;
        private string _selectedTime;

        private readonly List<string> _allTimeSlots = new List<string>
    {
        "09:00", "10:00", "11:00", "12:00", "13:00",
        "14:00", "15:00", "16:00", "17:00", "18:00"
    };

        public ObservableCollection<Service> AvailableServices { get; } = new ObservableCollection<Service>();
        public ObservableCollection<string> AvailableTimes { get; } = new ObservableCollection<string>();

        public event EventHandler<ServiceOrder> BookingAdded;

        public Service SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
                UpdateCanBook();
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                UpdateAvailableTimes();
                UpdateCanBook();
            }
        }

        public string SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
                OnPropertyChanged(nameof(SelectedTimeText));
                UpdateCanBook();
            }
        }

        public string SelectedTimeText => SelectedTime != null ? $"Выбрано время: {SelectedTime}" : "";

        public bool CanBook => SelectedService != null && SelectedDate.HasValue && !string.IsNullOrEmpty(SelectedTime);

        public ICommand CancelCommand { get; }
        public ICommand BookCommand { get; }
        public ICommand SelectTimeCommand { get; }

        public BookServiceViewModel(IDbContextFactory<CarWashDbContext> dbContextFactory, int clientId)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _clientId = clientId;

            var services = _dbContext.Услуги
                .Select(s => new Service
                {
                    ID_Услуги = s.ID_Услуги,
                    Наименование = $"{s.Наименование} ({s.Длительность} мин, {s.Стоимость} ₽)",
                    Длительность = s.Длительность,
                    Стоимость = s.Стоимость
                })
                .ToList();

            foreach (var service in services)
            {
                AvailableServices.Add(service);
            }

            CancelCommand = new RelayCommand(_ => _window?.Close());
            BookCommand = new RelayCommand(_ => Book());
            SelectTimeCommand = new RelayCommand(SelectTime);
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private void SelectTime(object parameter)
        {
            SelectedTime = parameter as string;
        }

        private void Book()
        {
            if (SelectedService != null && SelectedDate.HasValue && !string.IsNullOrEmpty(SelectedTime))
            {
                var bookingDateTime = DateTime.Parse($"{SelectedDate.Value:yyyy-MM-dd} {SelectedTime}");

                var employees = _dbContext.Сотрудники.ToList();
                if (!employees.Any())
                {
                    MessageBox.Show("Нет доступных сотрудников!", "Ошибка");
                    return;
                }

                var bookedSlots = _dbContext.Заказы_Услуг
                    .Include(so => so.Service)
                    .Where(so => so.Дата_и_время.Date == bookingDateTime.Date)
                    .ToList();

                var freeEmployee = employees.FirstOrDefault(emp =>
                    !bookedSlots.Any(slot =>
                        slot.ID_Сотрудника == emp.ID_Сотрудника &&
                        bookingDateTime < slot.Дата_и_время.AddMinutes(slot.Service.Длительность) &&
                        bookingDateTime.AddMinutes(SelectedService.Длительность) > slot.Дата_и_время));

                if (freeEmployee == null)
                {
                    MessageBox.Show("Нет свободных сотрудников на это время!", "Ошибка");
                    return;
                }

                var serviceOrder = new ServiceOrder
                {
                    ID_Услуги = SelectedService.ID_Услуги,
                    ID_Клиента = _clientId,
                    ID_Сотрудника = freeEmployee.ID_Сотрудника,
                    ID_Статуса = _dbContext.Статусы.FirstOrDefault(s => s.Название == "Ожидает")?.ID_Статуса ?? 1,
                    Дата_и_время = bookingDateTime
                };
                _dbContext.Заказы_Услуг.Add(serviceOrder);

                var visitHistory = new VisitHistory
                {
                    ID_Клиента = _clientId,
                    Дата_и_время = bookingDateTime
                };
                _dbContext.История_Посещений.Add(visitHistory);

                int bonusAmount = (int)(SelectedService.Стоимость * 0.05); // 5% от стоимости услуги
                var bonus = _dbContext.Бонусы
                    .Where(b => b.ID_Клиента == _clientId)
                    .OrderByDescending(b => b.ID_Бонуса)
                    .FirstOrDefault();

                if (bonus != null)
                {
                    bonus.Кол_во += bonusAmount;
                }
                else
                {
                    bonus = new Bonus
                    {
                        ID_Клиента = _clientId,
                        Кол_во = bonusAmount
                    };
                    _dbContext.Бонусы.Add(bonus);
                }

                _dbContext.SaveChanges();

                var bonusHistory = new BonusHistory
                {
                    ID_Бонус = bonus.ID_Бонуса,
                    Наименование = $"Начисление за услугу {SelectedService.Наименование}",
                    Кол_во = bonusAmount,
                    Тип = "Начисление",
                    Дата_и_время = DateTime.Now
                };
                _dbContext.История_Бонусов.Add(bonusHistory);

                _dbContext.SaveChanges();
                BookingAdded?.Invoke(this, serviceOrder);
                MessageBox.Show($"Вы записаны на {SelectedService.Наименование} на {bookingDateTime:dd.MM.yyyy HH:mm} (Сотрудник: {freeEmployee.Имя})");
                _window?.Close();
            }
        }

        private void UpdateAvailableTimes()
        {
            AvailableTimes.Clear();
            if (SelectedDate.HasValue && SelectedService != null)
            {
                var date = SelectedDate.Value.Date;
                var bookedSlots = _dbContext.Заказы_Услуг
                    .Include(so => so.Service)
                    .Where(so => so.Дата_и_время.Date == date)
                    .Select(so => new
                    {
                        StartTime = so.Дата_и_время,
                        Duration = so.Service.Длительность,
                        EmployeeId = so.ID_Сотрудника
                    })
                    .ToList();

                const int maxWashPlaces = 3; // Максимальное количество мест для мойки
                var employeeCount = _dbContext.Сотрудники.Count(); // Количество сотрудников

                foreach (var time in _allTimeSlots)
                {
                    var slotStart = DateTime.Parse($"{date:yyyy-MM-dd} {time}");
                    var slotEnd = slotStart.AddMinutes(SelectedService.Длительность);

                    // Проверяем, сколько сотрудников заняты в каждом временном слоте в периоде от slotStart до slotEnd
                    bool isSlotAvailable = true;
                    for (var currentTime = slotStart; currentTime < slotEnd; currentTime = currentTime.AddMinutes(60))
                    {
                        int occupiedEmployees = bookedSlots.Count(slot =>
                            currentTime < slot.StartTime.AddMinutes(slot.Duration) &&
                            currentTime.AddMinutes(60) > slot.StartTime);

                        if (occupiedEmployees >= employeeCount)
                        {
                            isSlotAvailable = false;
                            break;
                        }
                    }

                    if (isSlotAvailable)
                    {
                        AvailableTimes.Add(time);
                    }
                }

                if (SelectedTime != null && !AvailableTimes.Contains(SelectedTime))
                {
                    SelectedTime = null;
                }
            }
        }

        private void UpdateCanBook()
        {
            OnPropertyChanged(nameof(CanBook));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}