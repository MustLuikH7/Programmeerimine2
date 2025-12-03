using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, OperationResult<object>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);

            result.Value = new
            {
                AppointmentId = appointment.AppointmentId,
                DoctorId = appointment.DoctorId,
                UserId = appointment.UserId,
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt,
                CancelledAt = appointment.CancelledAt,
                Doctor = new
                {
                    FirstName = appointment.Doctor.FirstName,
                    LastName = appointment.Doctor.LastName,
                    Specialty = appointment.Doctor.Specialty
                },
                User = new
                {
                    FirstName = appointment.User.FirstName,
                    LastName = appointment.User.LastName,
                    Email = appointment.User.Email
                },
                Invoice = appointment.Invoice != null ? new
                {
                    InvoiceId = appointment.Invoice.InvoiceId,
                    IssuedAt = appointment.Invoice.IssuedAt,
                    IsPaid = appointment.Invoice.IsPaid
                } : null,
                Documents = appointment.Documents.Select(doc => new
                {
                    DocumentId = doc.DocumentId,
                    FileName = doc.FileName,
                    UploadedAt = doc.UploadedAt
                })
            };

            return result;
        }
    }
}