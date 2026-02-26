using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class UsersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Users/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<UserDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_user()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@test.com",
                PasswordHash = "hash",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Users/Get/?id={user.UserId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<UserDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(user.UserId, response.Value.UserId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_user()
        {
            // Arrange
            var url = "/api/Users/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_user()
        {
            // Arrange
            var url = "/api/Users/Delete/";

            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@test.com",
                PasswordHash = "hash",
                Phone = "123"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { userId = user.UserId })
            };
            using var response = await Client.SendAsync(request);
            var userFromDb = await DbContext.Users
                .Where(u => u.UserId == user.UserId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(userFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_user()
        {
            // Arrange
            var url = "/api/Users/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { userId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var url = "/api/Users/Save/";
            var command = new SaveUserCommand
            {
                UserId = 0,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@test.com",
                PasswordHash = "hash",
                Phone = "123"
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var userFromDb = await DbContext.Users
                .Where(u => u.UserId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(userFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_user()
        {
            // Arrange
            var url = "/api/Users/Save/";
            var command = new SaveUserCommand
            {
                UserId = 10,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@test.com",
                PasswordHash = "hash",
                Phone = "123"
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var userFromDb = await DbContext.Users
                .Where(u => u.UserId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(userFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_user()
        {
            // Arrange
            var url = "/api/Users/Save/";
            var command = new SaveUserCommand
            {
                UserId = 0,
                FirstName = "",
                LastName = "",
                Email = "",
                PasswordHash = "",
                Phone = ""
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var userFromDb = await DbContext.Users
                .Where(u => u.UserId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(userFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
