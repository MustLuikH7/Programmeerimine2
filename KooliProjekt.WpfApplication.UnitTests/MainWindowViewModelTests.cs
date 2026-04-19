using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Users { UserId = 1, FirstName = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Users { UserId = 1, FirstName = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users>
                {
                    Results = new List<Users>
                    {
                        new Users { UserId = 1, FirstName = "Test 1" },
                        new Users { UserId = 2, FirstName = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].UserId);
            Assert.Equal(2, _viewModel.Data[1].UserId);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Users>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.UserId);
        }

        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users>
                {
                    Results = new List<Users>
                    {
                        new Users { UserId = 1, FirstName = "Test 1" },
                        new Users { UserId = 2, FirstName = "Test 2" }
                    }
                }
            };
            var saveDataApiResult = new OperationResult();
            var listToSave = new Users { UserId = 1, FirstName = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Users>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(listToSave);

            // Arrange
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var saveDataApiResult = new OperationResult { Errors = new List<string> { "Error" } };
            var listToSave = new Users { UserId = 1, FirstName = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Users>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _dialogProviderMock.Setup(dp => dp.ShowError(It.IsAny<string>()))
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(listToSave);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.VerifyAll();
        }

        [Fact]
        public void SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Arrange
            _viewModel.SelectedItem = new Users { UserId = 1 };

            // Act
            var canExecute = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);

            _viewModel.SelectedItem = null;
            var cannotExecute = _viewModel.SaveCommand.CanExecute(null);
            Assert.False(cannotExecute);
        }

        [Fact]
        public void DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            _viewModel.SelectedItem = new Users { UserId = 1 };
            _dialogProviderMock.Setup(dp => dp.Confirm(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            _viewModel.SelectedItem = new Users { UserId = 1 };
            _dialogProviderMock.Setup(dp => dp.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(new OperationResult())
                .Verifiable();

            var loadDataApiResult = new OperationResult<PagedResult<Users>>
            {
                Value = new PagedResult<Users> { Results = new List<Users>() }
            };
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            _viewModel.SelectedItem = new Users { UserId = 1 };
            _dialogProviderMock.Setup(dp => dp.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(1))
                .ReturnsAsync(new OperationResult { Errors = new List<string> { "Error" } })
                .Verifiable();
            _dialogProviderMock.Setup(dp => dp.ShowError(It.IsAny<string>()))
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Arrange
            _viewModel.SelectedItem = new Users { UserId = 1 };

            // Act
            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);

            _viewModel.SelectedItem = new Users { UserId = 0 };
            var cannotExecuteIdZero = _viewModel.DeleteCommand.CanExecute(null);
            Assert.False(cannotExecuteIdZero);

            _viewModel.SelectedItem = null;
            var cannotExecuteNull = _viewModel.DeleteCommand.CanExecute(null);
            Assert.False(cannotExecuteNull);
        }
    }
}
