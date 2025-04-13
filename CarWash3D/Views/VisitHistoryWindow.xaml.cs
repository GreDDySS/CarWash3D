using CarWash3D.ViewModels;
using System.Windows;

namespace CarWash3D.Views
{
    public partial class VisitHistoryWindow : Window
    {
        public VisitHistoryWindow(VisitHistoryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

        }
    }
}
