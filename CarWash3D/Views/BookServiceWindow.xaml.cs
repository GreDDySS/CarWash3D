using CarWash3D.ViewModels;
using System.Windows;

namespace CarWash3D.Views
{
    
    public partial class BookServiceWindow : Window
    {
        public BookServiceWindow(BookServiceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
