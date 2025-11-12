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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var appointment = new Appointment();
            if(request.AppointmentId == 0)
            {
                await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
            }
            else
            {
                appointment = await _dbContext.Appointments.FindAsync(new object[] { request.AppointmentId }, cancellationToken);
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