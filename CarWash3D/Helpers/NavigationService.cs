using CarWash3D.Services;
using CarWash3D.ViewModels;
using CarWash3D.Views;
using System.Windows;
using System.Windows.Controls;

namespace CarWash3D.Helpers
{
    public class NavigationService
    {
        private readonly Frame _frame;
        private readonly ClientService _clientService;
        private readonly EmployeeService _employeeService;

        public NavigationService(Frame frame, ClientService clientService, EmployeeService employeeService)
        {
            _frame = frame;
            _clientService = clientService;
            _employeeService = employeeService;
        }

        public void NavigateTo(string pageName)
        {
            switch (pageName)
            {
                case "LoginView":
                    _frame.Navigate(new LoginView(new LoginViewModel(_clientService, _employeeService, this)));
                    break;
                case "RegistrationView":
                    _frame.Navigate(new RegistrationView(new RegistrationViewModel(_clientService, this)));
                    break;
                case "CustomerCabinetView":
                    // Заглушка для личного кабинета клиента
                    MessageBox.Show("Переход в личный кабинет клиента (пока не реализовано)");
                    break;
                case "DashboardView":
                    // Заглушка для панели сотрудника
                    MessageBox.Show("Переход в панель сотрудника (пока не реализовано)");
                    break;
            }
        }
    }
}
