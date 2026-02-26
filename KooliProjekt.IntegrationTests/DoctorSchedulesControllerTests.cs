using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.DoctorSchedules;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class DoctorSchedulesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/DoctorSchedules/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<DoctorScheduleDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_schedule()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var schedule = new DoctorSchedule
            {
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                ValidFrom = DateTime.Now
            };
            await DbContext.DoctorSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            var url = $"/api/DoctorSchedules/Get/?id={schedule.ScheduleId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<DoctorScheduleDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(schedule.ScheduleId, response.Value.ScheduleId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_schedule()
        {
            // Arrange
            var url = "/api/DoctorSchedules/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_schedule()
        {
            // Arrange
            var url = "/api/DoctorSchedules/Delete/";

            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var schedule = new DoctorSchedule
            {
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                ValidFrom = DateTime.Now
            };
            await DbContext.DoctorSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { scheduleId = schedule.ScheduleId })
            };
            using var response = await Client.SendAsync(request);
            var scheduleFromDb = await DbContext.DoctorSchedules
                .Where(s => s.ScheduleId == schedule.ScheduleId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(scheduleFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_schedule()
        {
            // Arrange
            var url = "/api/DoctorSchedules/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { scheduleId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_schedule()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var url = "/api/DoctorSchedules/Save/";
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var scheduleFromDb = await DbContext.DoctorSchedules
                .Where(s => s.ScheduleId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(scheduleFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_schedule()
        {
            // Arrange
            var doctor = new Doctor { FirstName = "John", LastName = "Doe", Email = "john@test.com", PasswordHash = "hash", Specialty = "General" };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var url = "/api/DoctorSchedules/Save/";
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 10,
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var scheduleFromDb = await DbContext.DoctorSchedules
                .Where(s => s.ScheduleId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(scheduleFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_schedule()
        {
            // Arrange
            var url = "/api/DoctorSchedules/Save/";
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = 0,
                DayOfWeek = "",
                StartTime = default,
                EndTime = default,
                ValidFrom = default
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var scheduleFromDb = await DbContext.DoctorSchedules
                .Where(s => s.ScheduleId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(scheduleFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
