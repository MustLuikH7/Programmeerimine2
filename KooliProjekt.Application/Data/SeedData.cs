using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Generate()
        {
            if (_dbContext.Users.Any())
            {
                return;
            }

            var users = new List<User>();
            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    FirstName = "UserFirst" + (i + 1),
                    LastName = "UserLast" + (i + 1),
                    Email = $"user{i + 1}@example.com",
                    PasswordHash = "dummyhash123",
                    Phone = "555-010" + i
                };
                users.Add(user);
                _dbContext.Users.Add(user);
            }

            var doctors = new List<Doctor>();
            string[] specialties = { "Cardiology", "Dermatology", "General Practice", "Neurology", "Orthopedics" };

            for (var i = 0; i < 10; i++)
            {
                var doctor = new Doctor
                {
                    FirstName = "Dr. " + (char)('A' + i),
                    LastName = "DoctorLast" + (i + 1),
                    Email = $"doctor{i + 1}@hospital.com",
                    PasswordHash = "dummyhash123",
                    Specialty = specialties[i % specialties.Length]
                };
                doctors.Add(doctor);
                _dbContext.Doctors.Add(doctor);
            }

            _dbContext.SaveChanges();

            foreach (var doc in doctors)
            {
                var schedule = new DoctorSchedule
                {
                    DoctorId = doc.DoctorId,
                    DayOfWeek = "Monday",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    ValidFrom = DateTime.UtcNow
                };
                _dbContext.DoctorSchedules.Add(schedule);
            }

            var random = new Random();

            foreach (var user in users)
            {
                var doctor = doctors[random.Next(doctors.Count)];

                var appointment = new Appointment
                {
                    UserId = user.UserId,
                    DoctorId = doctor.DoctorId,
                    AppointmentTime = DateTime.UtcNow.AddDays(random.Next(1, 30)),
                    Status = "Scheduled",
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Appointments.Add(appointment);

                var document = new Document
                {
                    Appointment = appointment,
                    DoctorId = doctor.DoctorId,
                    FileName = "referral_" + user.UserId + ".pdf",
                    FilePath = "/uploads/referral_" + user.UserId + ".pdf",
                    UploadedAt = DateTime.UtcNow
                };
                _dbContext.Documents.Add(document);

                var invoice = new Invoice
                {
                    Appointment = appointment,
                    DoctorId = doctor.DoctorId,
                    UserId = user.UserId,
                    IssuedAt = DateTime.UtcNow,
                    IsPaid = false
                };
                _dbContext.Invoices.Add(invoice);

                for (int k = 0; k < 2; k++)
                {
                    var item = new InvoiceItem
                    {
                        Invoice = invoice,
                        Description = $"Consultation Fee Part {k + 1}",
                        Amount = 50 * (k + 1)
                    };
                    _dbContext.InvoiceItems.Add(item);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}