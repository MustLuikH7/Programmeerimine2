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
        public async Task Get_should_return_null_value_when_request_is_null()
        {
            // Arrange
            var handler = new GetDoctorScheduleQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
