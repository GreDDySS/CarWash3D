using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CarWash3D;
using CarWash3D.ViewModels;
using CarWash3D.Views;
using Microsoft.Extensions.DependencyInjection;

public class NavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private Frame _frame; // Frame теперь устанавливается позже
    private readonly Dictionary<string, Type> _views;
    private Func<int, CustomerCabinetViewModel> _customerCabinetViewModelFactory;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _views = new Dictionary<string, Type>
        {
            { "LoginView", typeof(LoginView) },
            { "RegistrationView", typeof(RegistrationView) },
            { "CustomerCabinetView", typeof(CustomerCabinetView) }
        };
    }

    public void SetFrame(Frame frame)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    public void UpdateCustomerCabinetViewModelFactory(Func<int, CustomerCabinetViewModel> factory)
    {
        _customerCabinetViewModelFactory = factory;
    }

    public void NavigateTo(string viewName, object parameter = null)
    {
        if (_frame == null)
        {
            throw new InvalidOperationException("Frame is not set. Call SetFrame before navigating.");
        }

        if (_views.TryGetValue(viewName, out var viewType))
        {
            // Определяем размеры окна в зависимости от представления
            double height = 600; // Значение по умолчанию
            double width = 450;  // Значение по умолчанию

            switch (viewName)
            {
                case "LoginView":
                    height = 600;
                    width = 450;
                    break;
                case "RegistrationView":
                    height = 600;
                    width = 450;
                    break;
                case "CustomerCabinetView":
                    height = 600;
                    width = 1200;
                    break;
            }

            // Получаем MainWindow через Frame
            if (_frame.Parent is FrameworkElement element && Window.GetWindow(element) is Window window)
            {
                window.Height = height;
                window.Width = width;
            }

            object view;
            switch (viewName)
            {
                case "LoginView":
                    var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                    view = Activator.CreateInstance(viewType, loginViewModel);
                    break;

                case "RegistrationView":
                    var registrationViewModel = _serviceProvider.GetRequiredService<RegistrationViewModel>();
                    view = Activator.CreateInstance(viewType, registrationViewModel);
                    break;

                case "CustomerCabinetView":
                    if (parameter is int clientId)
                    {
                        if (_customerCabinetViewModelFactory == null)
                        {
                            throw new InvalidOperationException("CustomerCabinetViewModel factory not set.");
                        }
                        var customerCabinetViewModel = _customerCabinetViewModelFactory(clientId);
                        view = Activator.CreateInstance(viewType, customerCabinetViewModel);
                    }
                    else
                    {
                        throw new ArgumentException("CustomerCabinetView requires a clientId parameter of type int.");
                    }
                    break;

                default:
                    view = Activator.CreateInstance(viewType);
                    break;
            }

            _frame.Content = view;
        }
        else
        {
            throw new ArgumentException($"View {viewName} not found.");
        }
    }
}