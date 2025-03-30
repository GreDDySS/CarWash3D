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
            var parts = FullName.Split(' ');
            var client = new Client
            {
                Фамилия = parts[0],
                Имя = parts.Length > 1 ? parts[1] : "",
                Отчество = parts.Length > 2 ? parts[2] : "",
                Номер_телефона = Phone,
                Номер_машины = CarNumber
            };
            if (_clientService.RegisterClient(client))
            {
                MessageBox.Show("Регистрация выполнена успешно!");
                _navigationService.NavigateTo("LoginView");
            }
            else
            {
                MessageBox.Show("Ошибка регистрации. Проверьте данные и повторите попытку.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}