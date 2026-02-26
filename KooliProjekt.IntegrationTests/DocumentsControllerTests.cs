using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Documents;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class DocumentsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Documents/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<DocumentDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_document()
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

            var document = new Document
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test.pdf",
                FilePath = "/files/test.pdf",
                UploadedAt = DateTime.Now
            };
            await DbContext.Documents.AddAsync(document);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Documents/Get/?id={document.DocumentId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<DocumentDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(document.DocumentId, response.Value.DocumentId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_document()
        {
            // Arrange
            var url = "/api/Documents/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_document()
        {
            // Arrange
            var url = "/api/Documents/Delete/";

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

            var document = new Document
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test.pdf",
                FilePath = "/files/test.pdf",
                UploadedAt = DateTime.Now
            };
            await DbContext.Documents.AddAsync(document);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { documentId = document.DocumentId })
            };
            using var response = await Client.SendAsync(request);
            var documentFromDb = await DbContext.Documents
                .Where(d => d.DocumentId == document.DocumentId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(documentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_document()
        {
            // Arrange
            var url = "/api/Documents/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { documentId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_document()
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

            var url = "/api/Documents/Save/";
            var command = new SaveDocumentCommand
            {
                DocumentId = 0,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test.pdf",
                FilePath = "/files/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var documentFromDb = await DbContext.Documents
                .Where(d => d.DocumentId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(documentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_document()
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

            var url = "/api/Documents/Save/";
            var command = new SaveDocumentCommand
            {
                DocumentId = 10,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test.pdf",
                FilePath = "/files/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var documentFromDb = await DbContext.Documents
                .Where(d => d.DocumentId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(documentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_document()
        {
            // Arrange
            var url = "/api/Documents/Save/";
            var command = new SaveDocumentCommand
            {
                DocumentId = 0,
                AppointmentId = 0,
                DoctorId = 0,
                FileName = "",
                FilePath = "",
                UploadedAt = default
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var documentFromDb = await DbContext.Documents
                .Where(d => d.DocumentId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(documentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
