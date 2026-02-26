using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.InvoiceItems;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class InvoiceItemsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/InvoiceItems/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<InvoiceItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_invoice_item()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
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
                Description = "Consultation fee",
                Amount = 50
            };
            await DbContext.InvoiceItems.AddAsync(invoiceItem);
            await DbContext.SaveChangesAsync();

            var url = $"/api/InvoiceItems/Get/?id={invoiceItem.ItemId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<InvoiceItemDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(invoiceItem.ItemId, response.Value.ItemId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_invoice_item()
        {
            // Arrange
            var url = "/api/InvoiceItems/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_invoice_item()
        {
            // Arrange
            var url = "/api/InvoiceItems/Delete/";

            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
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
                Description = "Consultation fee",
                Amount = 50
            };
            await DbContext.InvoiceItems.AddAsync(invoiceItem);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { itemId = invoiceItem.ItemId })
            };
            using var response = await Client.SendAsync(request);
            var itemFromDb = await DbContext.InvoiceItems
                .Where(i => i.ItemId == invoiceItem.ItemId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(itemFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_invoice_item()
        {
            // Arrange
            var url = "/api/InvoiceItems/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { itemId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_invoice_item()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
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

            var url = "/api/InvoiceItems/Save/";
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = invoice.InvoiceId,
                Description = "Consultation fee",
                Amount = 50
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var itemFromDb = await DbContext.InvoiceItems
                .Where(i => i.ItemId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(itemFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_invoice_item()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
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

            var url = "/api/InvoiceItems/Save/";
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 10,
                InvoiceId = invoice.InvoiceId,
                Description = "Consultation fee",
                Amount = 50
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var itemFromDb = await DbContext.InvoiceItems
                .Where(i => i.ItemId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(itemFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_invoice_item()
        {
            // Arrange
            var url = "/api/InvoiceItems/Save/";
            var command = new SaveInvoiceItemCommand
            {
                ItemId = 0,
                InvoiceId = 0,
                Description = "",
                Amount = 0
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var itemFromDb = await DbContext.InvoiceItems
                .Where(i => i.ItemId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(itemFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
