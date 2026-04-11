using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;

        public MainWindowViewModel()
        {
            _apiClient = new ApiClient();
            Data = new ObservableCollection<Users>();
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
            Data = new ObservableCollection<Users>();
        }

        public ObservableCollection<Users> Data { get; }

        private Users _selectedItem;
        public Users SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                return;
            }

            Data.Clear();
            foreach (var item in response.Value.Results)
            {
                Data.Add(item);
            }
        }
    }
}
