using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult<PagedResult<Users>>();
            faultyResponse.Errors.Add("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users>
                {
                    Results = new List<Users>
                    {
                        new Users { UserId = 1, FirstName = "Test List 1" },
                        new Users { UserId = 2, FirstName = "Test List 2" }
                    }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            // Arrange
            var selectedList = (Users)null;

            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.FirstName = "").Verifiable();
            _mainViewMock.SetupSet(view => view.LastName = "").Verifiable();
            _mainViewMock.SetupSet(view => view.Email = "").Verifiable();
            _mainViewMock.SetupSet(view => view.Phone = "").Verifiable();




            // Act
            _presenter.SetSelection(selectedList);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var selectedList = (Users)null;

            _mainViewMock.SetupSet(view => view.CurrentId = 4).Verifiable();
            _mainViewMock.SetupSet(view => view.FirstName = "Martin").Verifiable();
            _mainViewMock.SetupSet(view => view.LastName = "Hansman").Verifiable();
            _mainViewMock.SetupSet(view => view.Email = "Martin.Hansman@techno.ee").Verifiable();
            _mainViewMock.SetupSet(view => view.Phone = "123456789").Verifiable();




            // Act
            _presenter.SetSelection(selectedList);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while saving data.");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Users>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // See mõelge ise välja
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // See mõelge ise välja
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // See mõelge ise välja
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // See mõelge ise välja
        }
    }
}