using CarWash3D.Data;
using CarWash3D.Helpers;
using CarWash3D.Services;
using CarWash3D.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CarWash3D
{
    public partial class MainWindow : Window
    {
        private readonly NavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            // Получаем NavigationService из ServiceProvider
            _navigationService = _serviceProvider.GetRequiredService<NavigationService>();

            // Устанавливаем Frame
            _navigationService.SetFrame(MainFrame);

            // Настраиваем фабрику CustomerCabinetViewModel
            var customerCabinetViewModelFactory = _serviceProvider.GetRequiredService<Func<int, CustomerCabinetViewModel>>();
            _navigationService.UpdateCustomerCabinetViewModelFactory(customerCabinetViewModelFactory);

            // Переходим на начальную страницу
            _navigationService.NavigateTo("LoginView");
        }
    }
}
