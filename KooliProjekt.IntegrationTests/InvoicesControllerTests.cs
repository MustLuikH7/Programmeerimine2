using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Invoices;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class InvoicesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Invoices/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<InvoiceDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_invoice()
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

            var url = $"/api/Invoices/Get/?id={invoice.InvoiceId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<InvoiceDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(invoice.InvoiceId, response.Value.InvoiceId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_invoice()
        {
            // Arrange
            var url = "/api/Invoices/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_invoice()
        {
            // Arrange
            var url = "/api/Invoices/Delete/";

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

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { invoiceId = invoice.InvoiceId })
            };
            using var response = await Client.SendAsync(request);
            var invoiceFromDb = await DbContext.Invoices
                .Where(i => i.InvoiceId == invoice.InvoiceId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(invoiceFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_invoice()
        {
            // Arrange
            var url = "/api/Invoices/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { invoiceId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_invoice()
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

            var url = "/api/Invoices/Save/";
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var invoiceFromDb = await DbContext.Invoices
                .Where(i => i.InvoiceId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(invoiceFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_invoice()
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

            var url = "/api/Invoices/Save/";
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 10,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var invoiceFromDb = await DbContext.Invoices
                .Where(i => i.InvoiceId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(invoiceFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_invoice()
        {
            // Arrange
            var url = "/api/Invoices/Save/";
            var command = new SaveInvoiceCommand
            {
                InvoiceId = 0,
                AppointmentId = 0,
                DoctorId = 0,
                UserId = 0,
                IssuedAt = default,
                IsPaid = false
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var invoiceFromDb = await DbContext.Invoices
                .Where(i => i.InvoiceId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(invoiceFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
