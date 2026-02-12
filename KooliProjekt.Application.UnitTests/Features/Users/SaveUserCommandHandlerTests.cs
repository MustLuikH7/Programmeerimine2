using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Users
{
    public class SaveUserCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveUserCommand)null;
            var handler = new SaveUserCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_when_id_is_negative()
        {
            // Arrange
            var request = new SaveUserCommand { UserId = -10 };
            var handler = new SaveUserCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "UserId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_user()
        {
            // Arrange
            var request = new SaveUserCommand
            {
                UserId = 0,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedUser = await DbContext.Users.SingleOrDefaultAsync(u => u.UserId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedUser);
            Assert.Equal(1, savedUser.UserId);
        }

        [Fact]
        public async Task Save_should_save_existing_user()
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

            var request = new SaveUserCommand
            {
                UserId = user.UserId,
                FirstName = "John",
                LastName = "Doe Updated",
                Email = "john.doe.updated@example.com",
                PasswordHash = "hashedpassword",
                Phone = "0987654321"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedUser = await DbContext.Users.SingleOrDefaultAsync(u => u.UserId == user.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedUser);
            Assert.Equal(request.Phone, savedUser.Phone);
        }

        [Fact]
        public async Task Save_should_return_error_if_user_does_not_exist()
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

            var request = new SaveUserCommand
            {
                UserId = 999,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData("", "Doe", "test@example.com")]
        [InlineData(null, "Doe", "test@example.com")]
        [InlineData("012345678901234567890123456789012345678901234567890", "Doe", "test@example.com")]
        public void SaveValidator_should_return_false_when_FirstName_is_invalid(string firstName, string lastName, string email)
        {
            // Arrange
            var validator = new SaveUserCommandValidator(DbContext);
            var command = new SaveUserCommand { UserId = 0, FirstName = firstName, LastName = lastName, Email = email };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveUserCommand.FirstName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("John", "", "test@example.com")]
        [InlineData("John", null, "test@example.com")]
        [InlineData("John", "012345678901234567890123456789012345678901234567890", "test@example.com")]
        public void SaveValidator_should_return_false_when_LastName_is_invalid(string firstName, string lastName, string email)
        {
            // Arrange
            var validator = new SaveUserCommandValidator(DbContext);
            var command = new SaveUserCommand { UserId = 0, FirstName = firstName, LastName = lastName, Email = email };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveUserCommand.LastName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("John", "Doe", "")]
        [InlineData("John", "Doe", null)]
        [InlineData("John", "Doe", "invalid-email")]
        public void SaveValidator_should_return_false_when_Email_is_invalid(string firstName, string lastName, string email)
        {
            // Arrange
            var validator = new SaveUserCommandValidator(DbContext);
            var command = new SaveUserCommand { UserId = 0, FirstName = firstName, LastName = lastName, Email = email };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveUserCommand.Email), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveUserCommandValidator(DbContext);
            var command = new SaveUserCommand
            {
                UserId = 0,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
