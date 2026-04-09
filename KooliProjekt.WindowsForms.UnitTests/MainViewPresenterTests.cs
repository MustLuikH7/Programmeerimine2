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
            faultyResponse.AddError("An error occurred while fetching data.");

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
            var selectedList = new Users
            {
                UserId = 4,
                FirstName = "Martin",
                LastName = "Hansman",
                Email = "Martin.Hansman@techno.ee",
                Phone = "123456789"
            };

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
            faultyResponse.AddError("An error occurred while saving data.");

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
            // Arrange
            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1);
            _mainViewMock.SetupGet(view => view.FirstName).Returns("Martin");
            _mainViewMock.SetupGet(view => view.LastName).Returns("Hansman");
            _mainViewMock.SetupGet(view => view.Email).Returns("Martin.Hansman@techno.ee");
            _mainViewMock.SetupGet(view => view.Phone).Returns("123456789");

            var validResponse = new OperationResult();

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Users>()))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var loadDataResponse = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users> { Results = new List<Users>() }
            };

            _apiClientMock
                .Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataResponse)
                .Verifiable();

            _mainViewMock
                .SetupSet(view => view.DataSource = loadDataResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(false).Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true).Verifiable();
            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1).Verifiable();

            var faultyResponse = new OperationResult();
            faultyResponse.AddError("Viga kustutamisel");

            _apiClientMock
                .Setup(client => client.Delete(1))
                .ReturnsAsync(faultyResponse)
                .Verifiable();

            _mainViewMock
                .Setup(view => view.ShowError("Viga kustutamisel", faultyResponse))
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // Arrange
            _mainViewMock.Setup(view => view.ConfirmDelete()).Returns(true).Verifiable();
            _mainViewMock.SetupGet(view => view.CurrentId).Returns(1).Verifiable();

            var validResponse = new OperationResult();

            _apiClientMock
                .Setup(client => client.Delete(1))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var loadDataResponse = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users> { Results = new List<Users>() }
            };

            _apiClientMock
                .Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataResponse)
                .Verifiable();

            _mainViewMock
                .SetupSet(view => view.DataSource = loadDataResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.VerifyAll();
        }
    }
}