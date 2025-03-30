using CarWash3D.Helpers;

namespace CarWash3D.ViewModels
{
    class MainViewModel
    {
        private readonly NavigationService _navigationService;

        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
