using CarWash3D.Data;
using CarWash3D.Data.Repositories;
using CarWash3D.Helpers;
using CarWash3D.Models;
using CarWash3D.Services;
using CarWash3D.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace CarWash3D
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Регистрируем DbContextFactory
            services.AddDbContextFactory<CarWashDbContext>(options =>
                options.UseSqlite("Data Source=Data/CarWash.db"));

            // Регистрируем репозитории
            services.AddScoped<ClientRepository>();
            services.AddScoped<EmployeeRepository>();

            // Регистрируем сервисы
            services.AddScoped<ClientService>();
            services.AddScoped<EmployeeService>();
            services.AddSingleton<NavigationService>();

            // Регистрируем ViewModel
            services.AddScoped<LoginViewModel>();
            services.AddScoped<RegistrationViewModel>();
            // Регистрируем фабрику для CustomerCabinetViewModel
            services.AddTransient<Func<int, CustomerCabinetViewModel>>(provider =>
            {
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<CarWashDbContext>>();
                var clientService = provider.GetRequiredService<ClientService>();
                var navigationService = provider.GetRequiredService<NavigationService>();
                return clientId => new CustomerCabinetViewModel(dbContextFactory, clientService, navigationService, clientId);
            });

            // Регистрируем фабрику для NavigationService

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow(ServiceProvider);
            mainWindow.Show();
        }
    }

}
