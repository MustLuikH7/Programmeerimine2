using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommandHandler : IRequestHandler<SaveAppointmentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAppointmentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.AppointmentId < 0)
            {
                result.AddPropertyError(nameof(request.AppointmentId), "Id cannot be negative");
                return result;
            }

            var appointment = new Appointment();

            if (request.AppointmentId == 0)
            {
                await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
            }
            else
            {
                appointment = await _dbContext.Appointments.FindAsync(new object[] { request.AppointmentId }, cancellationToken);
                if (appointment == null)
                {
                    result.AddError("Cannot find appointment with id " + request.AppointmentId);
                    return result;
                }
            }

            appointment.DoctorId = request.DoctorId;
            appointment.UserId = request.UserId;
            appointment.AppointmentTime = request.AppointmentTime;
            appointment.Status = request.Status;
            appointment.CreatedAt = request.CreatedAt;
            appointment.CancelledAt = request.CancelledAt;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}