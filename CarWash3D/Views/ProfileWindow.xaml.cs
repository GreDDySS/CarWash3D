using CarWash3D.ViewModels;
using System.Windows;

namespace CarWash3D.Views
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow(ProfileViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
