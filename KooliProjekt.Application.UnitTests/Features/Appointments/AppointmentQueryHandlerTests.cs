using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Appointments;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Appointments
{
    public class AppointmentQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_results()
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

            var query = new AppointmentsQuery { Page = 1, PageSize = 10 };
            var handler = new AppointmentQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.True(result.Value.Results.Count > 0);
        }

        [Fact]
        public async Task List_should_throw_ArgumentNullException_when_request_is_null()
        {
            // Arrange
            var handler = new AppointmentQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task List_should_throw_ArgumentException_when_page_is_zero_or_less(int page)
        {
            // Arrange
            var dbContext = GetFaultyDbContext();
            var query = new AppointmentsQuery { Page = page, PageSize = 10 };
            var handler = new AppointmentQueryHandler(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task List_should_throw_ArgumentException_when_pageSize_is_zero_or_less(int pageSize)
        {
            // Arrange
            var dbContext = GetFaultyDbContext();
            var query = new AppointmentsQuery { Page = 1, PageSize = pageSize };
            var handler = new AppointmentQueryHandler(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        [InlineData(1000)]
        public async Task List_should_throw_ArgumentException_when_pageSize_exceeds_max(int pageSize)
        {
            // Arrange
            var dbContext = GetFaultyDbContext();
            var query = new AppointmentsQuery { Page = 1, PageSize = pageSize };
            var handler = new AppointmentQueryHandler(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
