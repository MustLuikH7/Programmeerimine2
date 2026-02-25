using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Users
{
    public class DeleteUserCommandHandlerTests : TestBase
    {
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteUserCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteUserCommand)null;
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_should_return_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var command = new DeleteUserCommand { UserId = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteUserCommandHandler(faultyDbContext);

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_remove_existing_user()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var command = new DeleteUserCommand { UserId = user.UserId };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var deletedUser = await DbContext.Users.FindAsync(command.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task Delete_should_not_fail_when_user_does_not_exist()
        {
            // Arrange
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var command = new DeleteUserCommand { UserId = 999 };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var existingUser = await DbContext.Users.FindAsync(command.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(existingUser);
        }
    }
}
