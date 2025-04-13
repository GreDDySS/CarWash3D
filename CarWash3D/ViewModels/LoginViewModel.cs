using CarWash3D.Helpers;
using CarWash3D.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ClientService _clientService;
        private readonly EmployeeService _employeeService;
        private readonly NavigationService _navigationService;
        private bool _isClientTabActive = true;
        private int _activeTabIndex = 0;
        private bool _isCodeEntryVisible = false;
        private string _lastAuthenticatedPhone; // Для хранения номера телефона клиента после запроса кода

        public string ClientPhone { get; set; } = "+7 (999) 123-45-67";
        public string ClientCarNumber { get; set; } = "A123BВ777";
        public string EmployeeUsername { get; set; } = "admin";
        public string EmployeePassword { get; set; } = "admin";
        public string VerificationCode { get; set; } = "";

        public bool IsClientTabActive
        {
            get => _isClientTabActive;
            set
            {
                _isClientTabActive = value;
                OnPropertyChanged(nameof(IsClientTabActive));
                OnPropertyChanged(nameof(IsEmployeeTabActive));
                OnPropertyChanged(nameof(IsClientPanelVisible));
                OnPropertyChanged(nameof(IsEmployeePanelVisible));
            }
        }

        public bool IsEmployeeTabActive => !_isClientTabActive;

        public bool IsClientPanelVisible => _isClientTabActive && !_isCodeEntryVisible;

        public bool IsEmployeePanelVisible => !_isClientTabActive;

        public bool IsCodeEntryVisible
        {
            get => _isCodeEntryVisible;
            set
            {
                _isCodeEntryVisible = value;
                OnPropertyChanged(nameof(IsCodeEntryVisible));
                OnPropertyChanged(nameof(IsClientPanelVisible));
            }
        }

        public int ActiveTabIndex
        {
            get => _activeTabIndex;
            set
            {
                _activeTabIndex = value;
                OnPropertyChanged(nameof(ActiveTabIndex));
            }
        }

        public ICommand SwitchToClientCommand { get; }
        public ICommand SwitchToEmployeeCommand { get; }
        public ICommand ClientLoginCommand { get; }
        public ICommand EmployeeLoginCommand { get; }
        public ICommand NavigateToRegistrationCommand { get; }
        public ICommand VerifyCodeCommand { get; }

        public LoginViewModel(ClientService clientService, EmployeeService employeeService, NavigationService navigationService)
        {
            _clientService = clientService;
            _employeeService = employeeService;
            _navigationService = navigationService;

            SwitchToClientCommand = new RelayCommand(_ => SwitchToClientMode());
            SwitchToEmployeeCommand = new RelayCommand(_ => SwitchToEmployeeMode());
            ClientLoginCommand = new RelayCommand(_ => RequestVerificationCode());
            EmployeeLoginCommand = new RelayCommand(_ => LoginEmployee());
            NavigateToRegistrationCommand = new RelayCommand(_ => _navigationService.NavigateTo("RegistrationView"));
            VerifyCodeCommand = new RelayCommand(_ => VerifyCode());
        }

        private void SwitchToClientMode()
        {
            IsClientTabActive = true;
            ActiveTabIndex = 0;
            IsCodeEntryVisible = false;
        }

        private void SwitchToEmployeeMode()
        {
            IsClientTabActive = false;
            ActiveTabIndex = 1;
            IsCodeEntryVisible = false;
        }

        private void RequestVerificationCode()
        {
            if (_clientService.AuthenticateClient(ClientPhone, ClientCarNumber))
            {
                _lastAuthenticatedPhone = ClientPhone; // Сохраняем номер телефона для последующей проверки
                IsCodeEntryVisible = true;
            }
            else
            {
                MessageBox.Show("Неверные данные клиента.");
            }
        }

        private void VerifyCode()
        {
            if (_clientService.VerifyCode(_lastAuthenticatedPhone, VerificationCode))
            {
                var client = _clientService.GetClientByPhoneNumber(_lastAuthenticatedPhone);
                if (client != null)
                {
                    _navigationService.NavigateTo("CustomerCabinetView", client.ID_Клиента);
                    MessageBox.Show("Вход клиента выполнен!");
                    IsCodeEntryVisible = false;
                    VerificationCode = "";
                    _lastAuthenticatedPhone = null;
                }
                else
                {
                    MessageBox.Show("Ошибка: клиент не найден после верификации.");
                }
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения.");
            }
        }

        private void LoginEmployee()
        {
            if (_employeeService.AuthenticateEmployee(EmployeeUsername, EmployeePassword))
            {
                _navigationService.NavigateTo("DashboardView");
                MessageBox.Show("Вход сотрудника выполнен!");
            }
            else
            {
                MessageBox.Show("Неверные данные сотрудника.");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}