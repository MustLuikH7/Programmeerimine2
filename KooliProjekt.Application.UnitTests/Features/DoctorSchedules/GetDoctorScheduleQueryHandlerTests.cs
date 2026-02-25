using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorSchedules;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.DoctorSchedules
{
    public class GetDoctorScheduleQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task Get_should_return_object_if_object_exists()
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

            var query = new GetDoctorScheduleQuery { ScheduleId = schedule.ScheduleId };
            var handler = new GetDoctorScheduleQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Get_should_return_null_if_object_does_not_exist()
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

            var query = new GetDoctorScheduleQuery { ScheduleId = 999 };
            var handler = new GetDoctorScheduleQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_throw_ArgumentNullException_when_request_is_null()
        {
            // Arrange
            var handler = new GetDoctorScheduleQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_less(int id)
        {
            // Arrange
            var dbContext = GetFaultyDbContext();
            var query = new GetDoctorScheduleQuery { ScheduleId = id };
            var handler = new GetDoctorScheduleQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
