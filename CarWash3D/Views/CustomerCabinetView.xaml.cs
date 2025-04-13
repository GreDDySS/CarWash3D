using CarWash3D.ViewModels;
using System.Windows.Controls;

namespace CarWash3D.Views
{
    public partial class CustomerCabinetView : Page
    {
        public CustomerCabinetView(CustomerCabinetViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

        }
    }
}
