using CarWash3D.Data;
using CarWash3D.Data.Repositories;
using CarWash3D.Helpers;
using CarWash3D.Services;
using CarWash3D.ViewModels;
using CarWash3D.Views;
using System.Windows;

namespace CarWash3D
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация базы данных и репозиториев
            var dbContext = new CarWashDbContext();
            var clientRepository = new ClientRepository(dbContext);
            var employeeRepository = new EmployeeRepository(dbContext);

            // Инициализация сервисов
            var clientService = new ClientService(clientRepository);
            var employeeService = new EmployeeService(employeeRepository);
            _navigationService = new NavigationService(MainFrame, clientService, employeeService);

            DataContext = new MainViewModel(_navigationService);
            _navigationService.NavigateTo("LoginView");
        }
    }
}