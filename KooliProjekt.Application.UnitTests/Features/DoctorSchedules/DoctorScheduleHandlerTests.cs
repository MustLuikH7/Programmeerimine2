using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorSchedules;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.DoctorSchedules
{
    public class DoctorScheduleHandlerTests : TestBase
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
                Specialty = "General"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            var schedule = new DoctorSchedule
            {
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                ValidFrom = DateTime.Now
            };
            await DbContext.DoctorSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            var query = new DoctorSchedulesQuery { Page = 1, PageSize = 10 };
            var handler = new DoctorScheduleHandler(DbContext);

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
            var handler = new DoctorScheduleHandler(DbContext);

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
            var query = new DoctorSchedulesQuery { Page = page, PageSize = 10 };
            var handler = new DoctorScheduleHandler(dbContext);

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
            var query = new DoctorSchedulesQuery { Page = 1, PageSize = pageSize };
            var handler = new DoctorScheduleHandler(dbContext);

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
            var query = new DoctorSchedulesQuery { Page = 1, PageSize = pageSize };
            var handler = new DoctorScheduleHandler(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
