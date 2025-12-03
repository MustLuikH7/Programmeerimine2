using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommandHandler : IRequestHandler<SaveAppointmentCommand, OperationResult>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public SaveAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult> Handle(SaveAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                appointment = new Appointment();
            }

            appointment.DoctorId = request.DoctorId;
            appointment.UserId = request.UserId;
            appointment.AppointmentTime = request.AppointmentTime;
            appointment.Status = request.Status;
            appointment.CreatedAt = request.CreatedAt;
            appointment.CancelledAt = request.CancelledAt;

            await _appointmentRepository.SaveAsync(appointment);

            return result;
        }
    }
}