using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Doctors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Doctors
{
    public class SaveDoctorCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveDoctorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveDoctorCommand)null;
            var handler = new SaveDoctorCommandHandler(DbContext);

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
            var request = new SaveDoctorCommand { DoctorId = -10 };
            var handler = new SaveDoctorCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "DoctorId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_doctor()
        {
            // Arrange
            var request = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedDoctor = await DbContext.Doctors.SingleOrDefaultAsync(d => d.DoctorId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedDoctor);
            Assert.Equal(1, savedDoctor.DoctorId);
        }

        [Fact]
        public async Task Save_should_save_existing_doctor()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var request = new SaveDoctorCommand
            {
                DoctorId = doctor.DoctorId,
                FirstName = "Dr. John",
                LastName = "Smith Updated",
                Email = "dr.smith.updated@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Neurology"
            };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedDoctor = await DbContext.Doctors.SingleOrDefaultAsync(d => d.DoctorId == doctor.DoctorId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedDoctor);
            Assert.Equal(request.Specialty, savedDoctor.Specialty);
        }

        [Fact]
        public async Task Save_should_return_error_if_doctor_does_not_exist()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var request = new SaveDoctorCommand
            {
                DoctorId = 999,
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };
            var handler = new SaveDoctorCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_FirstName_is_invalid(string firstName)
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = firstName,
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorCommand.FirstName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_LastName_is_invalid(string lastName)
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = lastName,
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorCommand.LastName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid-email")]
        public void SaveValidator_should_return_false_when_Email_is_invalid(string email)
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = "Smith",
                Email = email,
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorCommand.Email), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_PasswordHash_is_invalid(string passwordHash)
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = passwordHash,
                Specialty = "Cardiology"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorCommand.PasswordHash), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_Specialty_is_invalid(string specialty)
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = specialty
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorCommand.Specialty), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveDoctorCommandValidator(DbContext);
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
