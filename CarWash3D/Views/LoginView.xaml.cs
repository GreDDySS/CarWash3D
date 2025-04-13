using CarWash3D.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarWash3D.Views
{
    public partial class LoginView : Page
    {
        private readonly LoginViewModel _viewModel;

        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.EmployeePassword = passwordBox.Password;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void PhoneValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var text = textBox.Text + e.Text;
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(text, @"^\+7\s\(\d{0,3}\)\s\d{0,3}-?\d{0,2}-?\d{0,2}$");
        }

        private void CarNumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var text = textBox.Text + e.Text;
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(text, @"^[А-ЯA-Z]?\d{0,3}[А-ЯA-Z]{0,2}\d{0,3}$");
        }
    }
}
