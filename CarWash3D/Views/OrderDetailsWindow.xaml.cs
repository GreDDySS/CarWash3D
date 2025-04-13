using CarWash3D.ViewModels;
using System.Windows;

namespace CarWash3D.Views
{
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow(OrderDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}