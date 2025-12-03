using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQueryHandler : IRequestHandler<GetDoctorQuery, OperationResult<object>>
    {
        private readonly IDoctorRepository _doctorRepository;

        public GetDoctorQueryHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

            result.Value = new
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email,
                Specialty = doctor.Specialty,
                Schedules = doctor.Schedules.Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                }),
                Appointments = doctor.Appointments.Select(a => new
                {
                    AppointmentId = a.AppointmentId,
                    UserId = a.UserId,
                    Status = a.Status
                }),
                Invoices = doctor.Invoices.Select(i => new
                {
                    InvoiceId = i.InvoiceId,
                    UserId = i.UserId,
                    IssuedAt = i.IssuedAt,
                    IsPaid = i.IsPaid
                }),
                Documents = doctor.Documents.Select(doc => new
                {
                    DocumentId = doc.DocumentId,
                    AppointmentId = doc.AppointmentId,
                    FileName = doc.FileName,
                    UploadedAt = doc.UploadedAt
                })
            };

            return result;
        }
    }
}