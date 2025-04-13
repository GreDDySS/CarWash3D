using CarWash3D.Data;
using CarWash3D.Helpers;
using CarWash3D.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly CarWashDbContext _dbContext;
        private readonly Client _client;
        private Window _window;

        private string _lastName;
        private string _firstName;
        private string _patronymic;
        private string _phoneNumber;
        private string _licensePlate;

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                _licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

        public ProfileViewModel(IDbContextFactory<CarWashDbContext> dbContextFactory, int clientId)
        {
            _dbContext = dbContextFactory.CreateDbContext();

            // Загружаем данные клиента
            _client = _dbContext.Клиенты.FirstOrDefault(c => c.ID_Клиента == clientId);

            if (_client != null)
            {
                LastName = _client.Фамилия;
                FirstName = _client.Имя;
                Patronymic = _client.Отчество;
                PhoneNumber = _client.Номер_телефона;
                LicensePlate = _client.Номер_машины;
            }

            CancelCommand = new RelayCommand(_ => _window?.Close());
            SaveCommand = new RelayCommand(_ => Save());
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private void Save()
        {
            if (_client != null)
            {
                _client.Фамилия = LastName;
                _client.Имя = FirstName;
                _client.Отчество = Patronymic;
                _client.Номер_телефона = PhoneNumber;
                _client.Номер_машины = LicensePlate;
                _dbContext.SaveChanges();
                MessageBox.Show("Данные успешно сохранены");
            }
            _window?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}