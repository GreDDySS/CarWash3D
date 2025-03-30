using CarWash3D.ViewModels;
using System.Windows.Controls;

namespace CarWash3D.Views
{
    public partial class RegistrationView : Page
    {
        public RegistrationView(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
