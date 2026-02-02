using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Documents;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Documents
{
    public class DeleteDocumentCommandHandlerTests : TestBase
    {
        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteDocumentCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteDocumentCommand)null;
            var handler = new DeleteDocumentCommandHandler(DbContext);

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
            var command = new DeleteDocumentCommand { DocumentId = id };
            var faultyDbContext = GetFaultyDbContext();
            var handler = new DeleteDocumentCommandHandler(faultyDbContext);

            var doctor = new Doctor
            {
                FirstName = "Dr. John",
                LastName = "Smith",
                Email = "dr.smith@example.com",
                PasswordHash = "hashedpassword",
                Specialty = "General"
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

            var document = new Document
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test_document.pdf",
                FilePath = "/documents/test_document.pdf",
                UploadedAt = DateTime.Now
            };
            await DbContext.Documents.AddAsync(document);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_remove_existing_document()
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

            var document = new Document
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test_document.pdf",
                FilePath = "/documents/test_document.pdf",
                UploadedAt = DateTime.Now
            };
            await DbContext.Documents.AddAsync(document);
            await DbContext.SaveChangesAsync();

            var command = new DeleteDocumentCommand { DocumentId = document.DocumentId };
            var handler = new DeleteDocumentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var deletedDocument = await DbContext.Documents.FindAsync(command.DocumentId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(deletedDocument);
        }

        [Fact]
        public async Task Delete_should_not_fail_when_document_does_not_exist()
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

            var document = new Document
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test_document.pdf",
                FilePath = "/documents/test_document.pdf",
                UploadedAt = DateTime.Now
            };
            await DbContext.Documents.AddAsync(document);
            await DbContext.SaveChangesAsync();

            var command = new DeleteDocumentCommand { DocumentId = 999 };
            var handler = new DeleteDocumentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var existingDocument = await DbContext.Documents.FindAsync(command.DocumentId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(existingDocument);
        }
    }
}
