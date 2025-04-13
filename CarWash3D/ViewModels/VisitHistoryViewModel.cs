using CarWash3D.Data;
using CarWash3D.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CarWash3D.ViewModels
{
    public class VisitHistoryViewModel : INotifyPropertyChanged
    {
        private readonly CarWashDbContext _dbContext;
        private Window _window;

        public ObservableCollection<Visit> Visits { get; } = new ObservableCollection<Visit>();

        public ICommand CloseCommand { get; }

        public VisitHistoryViewModel(IDbContextFactory<CarWashDbContext> dbContextFactory, int clientId)
        {
            _dbContext = dbContextFactory.CreateDbContext();

            var visits = _dbContext.История_Посещений
                .Where(v => v.ID_Клиента == clientId)
                .Select(v => new Visit
                {
                    Service = "Посещение", 
                    DateTime = v.Дата_и_время.ToString("dd.MM.yyyy HH:mm"),
                    Duration = "Не указано" 
                })
                .ToList();

            foreach (var visit in visits)
            {
                Visits.Add(visit);
            }

            CloseCommand = new RelayCommand(_ => _window?.Close());
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Visit
    {
        public string Service { get; set; }
        public string DateTime { get; set; }
        public string Duration { get; set; }
    }
}