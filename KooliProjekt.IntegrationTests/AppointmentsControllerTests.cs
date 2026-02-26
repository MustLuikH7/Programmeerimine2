using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Appointments;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class AppointmentsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Appointments/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AppointmentDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_appointment()
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

            var url = $"/api/Appointments/Get/?id={appointment.AppointmentId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<AppointmentDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(appointment.AppointmentId, response.Value.AppointmentId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_appointment()
        {
            // Arrange
            var url = "/api/Appointments/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_appointment()
        {
            // Arrange
            var url = "/api/Appointments/Delete/";

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

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { appointmentId = appointment.AppointmentId })
            };
            using var response = await Client.SendAsync(request);
            var appointmentFromDb = await DbContext.Appointments
                .Where(a => a.AppointmentId == appointment.AppointmentId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(appointmentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_appointment()
        {
            // Arrange
            var url = "/api/Appointments/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { appointmentId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_appointment()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = "/api/Appointments/Save/";
            var command = new SaveAppointmentCommand
            {
                AppointmentId = 0,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var appointmentFromDb = await DbContext.Appointments
                .Where(a => a.AppointmentId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(appointmentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_appointment()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var user = new User { FirstName = "Jane", LastName = "Doe", Email = "jane@test.com", PasswordHash = "hash", Phone = "123" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = "/api/Appointments/Save/";
            var command = new SaveAppointmentCommand
            {
                AppointmentId = 10,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                AppointmentTime = DateTime.Now,
                Status = "Scheduled",
                CreatedAt = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var appointmentFromDb = await DbContext.Appointments
                .Where(a => a.AppointmentId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(appointmentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_appointment()
        {
            // Arrange
            var url = "/api/Appointments/Save/";
            var command = new SaveAppointmentCommand
            {
                AppointmentId = 0,
                DoctorId = 0,
                UserId = 0,
                AppointmentTime = default,
                Status = "",
                CreatedAt = default
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var appointmentFromDb = await DbContext.Appointments
                .Where(a => a.AppointmentId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(appointmentFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
