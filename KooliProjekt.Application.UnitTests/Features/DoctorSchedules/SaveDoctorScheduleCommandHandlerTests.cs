using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.DoctorSchedules;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.DoctorSchedules
{
    public class SaveDoctorScheduleCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveDoctorScheduleCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveDoctorScheduleCommand)null;
            var handler = new SaveDoctorScheduleCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_when_id_is_negative()
        {
            // Arrange
            var request = new SaveDoctorScheduleCommand { ScheduleId = -10 };
            var handler = new SaveDoctorScheduleCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "ScheduleId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_schedule()
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

            var request = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                ValidFrom = DateTime.Now
            };
            var handler = new SaveDoctorScheduleCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedSchedule = await DbContext.DoctorSchedules.SingleOrDefaultAsync(s => s.ScheduleId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedSchedule);
            Assert.Equal(1, savedSchedule.ScheduleId);
        }

        [Fact]
        public async Task Save_should_save_existing_schedule()
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

            var request = new SaveDoctorScheduleCommand
            {
                ScheduleId = schedule.ScheduleId,
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Tuesday",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
                ValidFrom = DateTime.Now
            };
            var handler = new SaveDoctorScheduleCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedSchedule = await DbContext.DoctorSchedules.SingleOrDefaultAsync(s => s.ScheduleId == schedule.ScheduleId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedSchedule);
            Assert.Equal(request.DayOfWeek, savedSchedule.DayOfWeek);
        }

        [Fact]
        public async Task Save_should_return_error_if_schedule_does_not_exist()
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

            var request = new SaveDoctorScheduleCommand
            {
                ScheduleId = 999,
                DoctorId = doctor.DoctorId,
                DayOfWeek = "Tuesday",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
                ValidFrom = DateTime.Now
            };
            var handler = new SaveDoctorScheduleCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_DoctorId_is_invalid(int doctorId)
        {
            // Arrange
            var validator = new SaveDoctorScheduleCommandValidator(DbContext);
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = doctorId,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorScheduleCommand.DoctorId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890")]
        public void SaveValidator_should_return_false_when_DayOfWeek_is_invalid(string dayOfWeek)
        {
            // Arrange
            var validator = new SaveDoctorScheduleCommandValidator(DbContext);
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = 1,
                DayOfWeek = dayOfWeek,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorScheduleCommand.DayOfWeek), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_EndTime_is_before_StartTime()
        {
            // Arrange
            var validator = new SaveDoctorScheduleCommandValidator(DbContext);
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = 1,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(17, 0, 0),
                EndTime = new TimeSpan(9, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDoctorScheduleCommand.EndTime), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveDoctorScheduleCommandValidator(DbContext);
            var command = new SaveDoctorScheduleCommand
            {
                ScheduleId = 0,
                DoctorId = 1,
                DayOfWeek = "Monday",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                ValidFrom = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
