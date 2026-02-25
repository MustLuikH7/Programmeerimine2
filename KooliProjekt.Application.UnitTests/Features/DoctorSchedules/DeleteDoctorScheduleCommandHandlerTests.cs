using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorSchedules;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.DoctorSchedules
{
    public class DeleteDoctorScheduleCommandHandlerTests : TestBase
    {
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteDoctorScheduleCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteDoctorScheduleCommand)null;
            var handler = new DeleteDoctorScheduleCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_should_return_when_request_id_is_null_or_negative(int id)
        {
            // Arrange
            var command = new DeleteDoctorScheduleCommand { ScheduleId = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteDoctorScheduleCommandHandler(faultyDbContext);

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

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_remove_existing_schedule()
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

            var command = new DeleteDoctorScheduleCommand { ScheduleId = schedule.ScheduleId };
            var handler = new DeleteDoctorScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var deletedSchedule = await DbContext.DoctorSchedules.FindAsync(command.ScheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(deletedSchedule);
        }

        [Fact]
        public async Task Delete_should_not_fail_when_schedule_does_not_exist()
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

            var command = new DeleteDoctorScheduleCommand { ScheduleId = 999 };
            var handler = new DeleteDoctorScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var existingSchedule = await DbContext.DoctorSchedules.FindAsync(command.ScheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(existingSchedule);
        }
    }
}
