using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms

{

    public class MainViewPresenter

    {

        private readonly IApiClient _apiClient;

        private readonly IMainView _mainView;

        private Users _selectedList;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)

        {

            _apiClient = apiClient;

            _mainView = mainView;

            _mainView.SetPresenter(this);

        }

        public async Task LoadData()

        {

            var response = await _apiClient.List(1, 100);

            if (response.HasErrors)

            {

                _mainView.ShowError("Viga andmete laadimisel", response);

                _mainView.DataSource = null;

                return;

            }

            _mainView.DataSource = response.Value.Results;

        }

        public void SetSelection(Users selectedList)

        {

            _selectedList = selectedList;

            if (_selectedList == null)

            {

                _mainView.CurrentId = 0;

                _mainView.FirstName = "";

                _mainView.LastName = "";

                _mainView.Email = "";

                _mainView.Phone = "";

            }

            else

            {

                _mainView.CurrentId = _selectedList.UserId;

                _mainView.FirstName = _selectedList.FirstName;

                _mainView.LastName = _selectedList.LastName;

                _mainView.Email = _selectedList.Email;

                _mainView.Phone = _selectedList.Phone;




            }

        }

    }

}
