using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Appointments;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Appointments
{
    public class SaveAppointmentCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveAppointmentCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveAppointmentCommand)null;
            var handler = new SaveAppointmentCommandHandler(DbContext);

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
            var request = new SaveAppointmentCommand { AppointmentId = -10 };
            var handler = new SaveAppointmentCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "AppointmentId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_appointment()
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

            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var request = new SaveAppointmentCommand
            {
                AppointmentId = 0,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now.AddDays(1),
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveAppointmentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAppointment = await DbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAppointment);
            Assert.Equal(1, savedAppointment.AppointmentId);
        }

        [Fact]
        public async Task Save_should_save_existing_appointment()
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

            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now.AddDays(1),
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };
            await DbContext.Appointments.AddAsync(appointment);
            await DbContext.SaveChangesAsync();

            var request = new SaveAppointmentCommand
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now.AddDays(2),
                Status = "Confirmed",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveAppointmentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAppointment = await DbContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAppointment);
            Assert.Equal(request.Status, savedAppointment.Status);
        }

        [Fact]
        public async Task Save_should_return_error_if_appointment_does_not_exist()
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

            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PasswordHash = "hashedpassword",
                Phone = "1234567890"
            };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now.AddDays(1),
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };
            await DbContext.Appointments.AddAsync(appointment);
            await DbContext.SaveChangesAsync();

            var request = new SaveAppointmentCommand
            {
                AppointmentId = 999,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now.AddDays(2),
                Status = "Confirmed",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveAppointmentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_DoctorId_is_invalid(int doctorId)
        {
            // Arrange
            var validator = new SaveAppointmentCommandValidator(DbContext);
            var command = new SaveAppointmentCommand 
            { 
                AppointmentId = 0, 
                DoctorId = doctorId, 
                UserId = 1, 
                AppointmentTime = DateTime.Now.AddDays(1), 
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAppointmentCommand.DoctorId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_UserId_is_invalid(int userId)
        {
            // Arrange
            var validator = new SaveAppointmentCommandValidator(DbContext);
            var command = new SaveAppointmentCommand 
            { 
                AppointmentId = 0, 
                DoctorId = 1, 
                UserId = userId, 
                AppointmentTime = DateTime.Now.AddDays(1), 
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAppointmentCommand.UserId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_Status_is_invalid(string status)
        {
            // Arrange
            var validator = new SaveAppointmentCommandValidator(DbContext);
            var command = new SaveAppointmentCommand 
            { 
                AppointmentId = 0, 
                DoctorId = 1, 
                UserId = 1, 
                AppointmentTime = DateTime.Now.AddDays(1), 
                Status = status,
                CreatedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAppointmentCommand.Status), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveAppointmentCommandValidator(DbContext);
            var command = new SaveAppointmentCommand 
            { 
                AppointmentId = 0, 
                DoctorId = 1, 
                UserId = 1, 
                AppointmentTime = DateTime.Now.AddDays(1), 
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
