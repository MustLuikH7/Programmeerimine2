using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.InvoiceItems;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.InvoiceItems
{
    public class SaveInvoiceItemCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveInvoiceItemCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveInvoiceItemCommand)null;
            var handler = new SaveInvoiceItemCommandHandler(DbContext);

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
            var request = new SaveInvoiceItemCommand { ItemId = -10 };
            var handler = new SaveInvoiceItemCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "ItemId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_invoice_item()
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

            var request = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = invoice.InvoiceId,
                Description = "Consultation Fee",
                Amount = 100
            };
            var handler = new SaveInvoiceItemCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedItem = await DbContext.InvoiceItems.SingleOrDefaultAsync(i => i.ItemId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedItem);
            Assert.Equal(1, savedItem.ItemId);
        }

        [Fact]
        public async Task Save_should_save_existing_invoice_item()
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

            var invoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.InvoiceId,
                Description = "Consultation Fee",
                Amount = 100
            };
            await DbContext.InvoiceItems.AddAsync(invoiceItem);
            await DbContext.SaveChangesAsync();

            var request = new SaveInvoiceItemCommand
            {
                ItemId = invoiceItem.ItemId,
                InvoiceId = invoice.InvoiceId,
                Description = "Updated Consultation Fee",
                Amount = 150
            };
            var handler = new SaveInvoiceItemCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedItem = await DbContext.InvoiceItems.SingleOrDefaultAsync(i => i.ItemId == invoiceItem.ItemId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedItem);
            Assert.Equal(request.Description, savedItem.Description);
            Assert.Equal(request.Amount, savedItem.Amount);
        }

        [Fact]
        public async Task Save_should_return_error_if_invoice_item_does_not_exist()
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

            var invoiceItem = new InvoiceItem
            {
                InvoiceId = invoice.InvoiceId,
                Description = "Consultation Fee",
                Amount = 100
            };
            await DbContext.InvoiceItems.AddAsync(invoiceItem);
            await DbContext.SaveChangesAsync();

            var request = new SaveInvoiceItemCommand
            {
                ItemId = 999,
                InvoiceId = invoice.InvoiceId,
                Description = "Updated Consultation Fee",
                Amount = 150
            };
            var handler = new SaveInvoiceItemCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_InvoiceId_is_invalid(int invoiceId)
        {
            // Arrange
            var validator = new SaveInvoiceItemCommandValidator(DbContext);
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = invoiceId,
                Description = "Test Item",
                Amount = 100
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceItemCommand.InvoiceId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_Description_is_invalid(string description)
        {
            // Arrange
            var validator = new SaveInvoiceItemCommandValidator(DbContext);
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = 1,
                Description = description,
                Amount = 100
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceItemCommand.Description), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_Amount_is_invalid(int amount)
        {
            // Arrange
            var validator = new SaveInvoiceItemCommandValidator(DbContext);
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = 1,
                Description = "Test Item",
                Amount = amount
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceItemCommand.Amount), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveInvoiceItemCommandValidator(DbContext);
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = 1,
                Description = "Test Item",
                Amount = 100
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
