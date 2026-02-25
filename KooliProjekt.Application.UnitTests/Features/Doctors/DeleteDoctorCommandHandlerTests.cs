using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Doctors;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Doctors
{
    public class DeleteDoctorCommandHandlerTests : TestBase
    {
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteDoctorCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteDoctorCommand)null;
            var handler = new DeleteDoctorCommandHandler(DbContext);

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
            var command = new DeleteDoctorCommand { DoctorId = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteDoctorCommandHandler(faultyDbContext);

            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "Cardiology"
            };
            await DbContext.Doctors.AddAsync(doctor);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_remove_existing_doctor()
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
            await DbContext.SaveChangesAsync();

            var command = new DeleteDoctorCommand { DoctorId = doctor.DoctorId };
            var handler = new DeleteDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var deletedDoctor = await DbContext.Doctors.FindAsync(command.DoctorId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(deletedDoctor);
        }

        [Fact]
        public async Task Delete_should_not_fail_when_doctor_does_not_exist()
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
            await DbContext.SaveChangesAsync();

            var command = new DeleteDoctorCommand { DoctorId = 999 };
            var handler = new DeleteDoctorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var existingDoctor = await DbContext.Doctors.FindAsync(command.DoctorId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(existingDoctor);
        }
    }
}
