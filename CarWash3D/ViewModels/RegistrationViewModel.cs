using CarWash3D.Helpers;
using CarWash3D.Models;
using CarWash3D.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private readonly ClientService _clientService;
        private readonly NavigationService _navigationService;

        public string FullName { get; set; } = "Иванов Иван Иванович";
        public string Phone { get; set; } = "+7 (999) 123-45-67";
        public string CarNumber { get; set; } = "A123BВ777";

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public RegistrationViewModel(ClientService clientService, NavigationService navigationService)
        {
            _clientService = clientService;
            _navigationService = navigationService;

            RegisterCommand = new RelayCommand(_ => Register());
            NavigateToLoginCommand = new RelayCommand(_ => _navigationService.NavigateTo("LoginView"));
        }

        private void Register()
        {
            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(FullName) || !FullName.Contains(" "))
            {
                MessageBox.Show("Пожалуйста, введите корректное ФИО (Фамилия Имя Отчество).");
                return;
            }

            var nameParts = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 2 || nameParts.Length > 3)
            {
                MessageBox.Show("ФИО должно содержать минимум Фамилию и Имя.");
                return;
            }

            // Валидация номера телефона
            if (string.IsNullOrWhiteSpace(Phone) || !System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$"))
            {
                MessageBox.Show("Пожалуйста, введите номер телефона в формате +7 (XXX) XXX-XX-XX.");
                return;
            }

            // Валидация номера автомобиля
            if (string.IsNullOrWhiteSpace(CarNumber) || !System.Text.RegularExpressions.Regex.IsMatch(CarNumber, @"^[А-ЯA-Z]\d{3}[А-ЯA-Z]{2}\d{2,3}$"))
            {
                MessageBox.Show("Пожалуйста, введите номер автомобиля в формате A123BC45 или A123BC456.");
                return;
            }

            var client = new Client
            {
                Фамилия = nameParts[0],
                Имя = nameParts[1],
                Отчество = nameParts.Length > 2 ? nameParts[2] : "",
                Номер_телефона = Phone,
                Номер_машины = CarNumber
            };

            if (_clientService.RegisterClient(client))
            {
                MessageBox.Show("Регистрация прошла успешно!");
                _navigationService.NavigateTo("LoginView");
            }
            else
            {
                MessageBox.Show("Ошибка при регистрации. Возможно, такой клиент уже существует.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}