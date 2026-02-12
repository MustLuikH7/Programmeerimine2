using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Documents;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Documents
{
    public class SaveDocumentCommandHandlerTests : TestBase
    {
        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveDocumentCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveDocumentCommand)null;
            var handler = new SaveDocumentCommandHandler(DbContext);

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
            var request = new SaveDocumentCommand { DocumentId = -10 };
            var handler = new SaveDocumentCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var hasIdError = result.PropertyErrors.Any(e => e.Key == "DocumentId");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.True(hasIdError);
        }

        [Fact]
        public async Task Save_should_save_new_document()
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

            var request = new SaveDocumentCommand
            {
                DocumentId = 0,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "test_document.pdf",
                FilePath = "/documents/test_document.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveDocumentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedDocument = await DbContext.Documents.SingleOrDefaultAsync(d => d.DocumentId == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedDocument);
            Assert.Equal(1, savedDocument.DocumentId);
        }

        [Fact]
        public async Task Save_should_save_existing_document()
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

            var request = new SaveDocumentCommand
            {
                DocumentId = document.DocumentId,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "updated_document.pdf",
                FilePath = "/documents/updated_document.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveDocumentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);
            var savedDocument = await DbContext.Documents.SingleOrDefaultAsync(d => d.DocumentId == document.DocumentId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedDocument);
            Assert.Equal(request.FileName, savedDocument.FileName);
        }

        [Fact]
        public async Task Save_should_return_error_if_document_does_not_exist()
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

            var request = new SaveDocumentCommand
            {
                DocumentId = 999,
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                FileName = "updated_document.pdf",
                FilePath = "/documents/updated_document.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveDocumentCommandHandler(DbContext);

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_AppointmentId_is_invalid(int appointmentId)
        {
            // Arrange
            var validator = new SaveDocumentCommandValidator(DbContext);
            var command = new SaveDocumentCommand 
            { 
                DocumentId = 0, 
                AppointmentId = appointmentId, 
                DoctorId = 1,
                FileName = "test.pdf",
                FilePath = "/documents/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDocumentCommand.AppointmentId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_DoctorId_is_invalid(int doctorId)
        {
            // Arrange
            var validator = new SaveDocumentCommandValidator(DbContext);
            var command = new SaveDocumentCommand 
            { 
                DocumentId = 0, 
                AppointmentId = 1, 
                DoctorId = doctorId,
                FileName = "test.pdf",
                FilePath = "/documents/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDocumentCommand.DoctorId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_FileName_is_invalid(string fileName)
        {
            // Arrange
            var validator = new SaveDocumentCommandValidator(DbContext);
            var command = new SaveDocumentCommand 
            { 
                DocumentId = 0, 
                AppointmentId = 1, 
                DoctorId = 1,
                FileName = fileName,
                FilePath = "/documents/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDocumentCommand.FileName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_FilePath_is_invalid(string filePath)
        {
            // Arrange
            var validator = new SaveDocumentCommandValidator(DbContext);
            var command = new SaveDocumentCommand 
            { 
                DocumentId = 0, 
                AppointmentId = 1, 
                DoctorId = 1,
                FileName = "test.pdf",
                FilePath = filePath,
                UploadedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveDocumentCommand.FilePath), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_data_is_valid()
        {
            // Arrange
            var validator = new SaveDocumentCommandValidator(DbContext);
            var command = new SaveDocumentCommand 
            { 
                DocumentId = 0, 
                AppointmentId = 1, 
                DoctorId = 1,
                FileName = "test.pdf",
                FilePath = "/documents/test.pdf",
                UploadedAt = DateTime.Now
            };

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
