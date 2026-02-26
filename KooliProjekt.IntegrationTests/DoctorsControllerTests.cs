using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Doctors;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class DoctorsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Doctors/List/?page=1&pageSize=5";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<DoctorDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_doctor()
        {
            // Arrange
            var doctor = new Doctor
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@test.com",
                PasswordHash = "hash",
                Specialty = "Cardiology"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Doctors/Get/?id={doctor.DoctorId}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<DoctorDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(doctor.DoctorId, response.Value.DoctorId);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Get/?id=99999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_remove_existing_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Delete/";

            var doctor = new Doctor
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PasswordHash = "hash",
                Specialty = "General"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { doctorId = doctor.DoctorId })
            };
            using var response = await Client.SendAsync(request);
            var doctorFromDb = await DbContext.Doctors
                .Where(d => d.DoctorId == doctor.DoctorId)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(doctorFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { doctorId = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Save/";
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PasswordHash = "hash",
                Specialty = "General"
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var doctorFromDb = await DbContext.Doctors
                .Where(d => d.DoctorId == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(doctorFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Save/";
            var command = new SaveDoctorCommand
            {
                DoctorId = 10,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PasswordHash = "hash",
                Specialty = "General"
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var doctorFromDb = await DbContext.Doctors
                .Where(d => d.DoctorId == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(doctorFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_doctor()
        {
            // Arrange
            var url = "/api/Doctors/Save/";
            var command = new SaveDoctorCommand
            {
                DoctorId = 0,
                FirstName = "",
                LastName = "",
                Email = "",
                PasswordHash = "",
                Specialty = ""
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var doctorFromDb = await DbContext.Doctors
                .Where(d => d.DoctorId == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(doctorFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.True(result.HasErrors);
        }
    }
}
