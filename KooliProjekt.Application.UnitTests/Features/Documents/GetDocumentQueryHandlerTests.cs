using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Documents;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Documents
{
    public class GetDocumentQueryHandlerTests : TestBase
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

            var query = new GetDocumentQuery { DocumentId = document.DocumentId };
            var handler = new GetDocumentQueryHandler(DbContext);

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

            var query = new GetDocumentQuery { DocumentId = 999 };
            var handler = new GetDocumentQueryHandler(DbContext);

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
            var handler = new GetDocumentQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
