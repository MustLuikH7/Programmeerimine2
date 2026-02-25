using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Invoices;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features.Invoices
{
    public class GetInvoiceQueryHandlerTests : TestBase
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

            var invoice = new Invoice
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var query = new GetInvoiceQuery { InvoiceId = invoice.InvoiceId };
            var handler = new GetInvoiceQueryHandler(DbContext);

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

            var invoice = new Invoice
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = doctor.DoctorId,
                UserId = user.UserId,
                IssuedAt = DateTime.Now,
                IsPaid = false
            };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var query = new GetInvoiceQuery { InvoiceId = 999 };
            var handler = new GetInvoiceQueryHandler(DbContext);

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
            var handler = new GetInvoiceQueryHandler(DbContext);

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
            var query = new GetInvoiceQuery { InvoiceId = id };
            var handler = new GetInvoiceQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
