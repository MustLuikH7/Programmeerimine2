using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Invoices;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Invoices
{
    public class SaveInvoiceCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveInvoiceCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveInvoiceCommand)null;
            var handler = new SaveInvoiceCommandHandler(DbContext);

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
            var request = new SaveInvoiceCommand { InvoiceId = -10 };
            var handler = new SaveInvoiceCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "InvoiceId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_invoice()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "General"
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

            var request = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedInvoice = await DbContext.Invoices.SingleOrDefaultAsync(i => i.InvoiceId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedInvoice);
            Assert.Equal(1, savedInvoice.InvoiceId);
        }

        [Fact]
        public async Task Save_should_save_existing_invoice()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "General"
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

            var invoice = new Invoice
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var request = new SaveInvoiceCommand
            {
                InvoiceId = invoice.InvoiceId,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = true
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedInvoice = await DbContext.Invoices.SingleOrDefaultAsync(i => i.InvoiceId == invoice.InvoiceId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedInvoice);
            Assert.Equal(request.IsPaid, savedInvoice.IsPaid);
        }

        [Fact]
        public async Task Save_should_return_error_if_invoice_does_not_exist()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "General"
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

            var invoice = new Invoice
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var request = new SaveInvoiceCommand
            {
                InvoiceId = 999,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = true
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_AppointmentId_is_invalid(int appointmentId)
        {
            // Arrange
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = appointmentId,
                DoctorId = 1,
                UserId = 1,
                IssuedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceCommand.AppointmentId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_DoctorId_is_invalid(int doctorId)
        {
            // Arrange
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = 1,
                DoctorId = doctorId,
                UserId = 1,
                IssuedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceCommand.DoctorId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_UserId_is_invalid(int userId)
        {
            // Arrange
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = 1,
                DoctorId = 1,
                UserId = userId,
                IssuedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceCommand.UserId), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = 1,
                DoctorId = 1,
                UserId = 1,
                IssuedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
