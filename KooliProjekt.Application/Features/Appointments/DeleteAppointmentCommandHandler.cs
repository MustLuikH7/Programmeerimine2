using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Appointments
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAppointmentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.AppointmentId <= 0)
            {
                return result;
            }

            var appointment = await _dbContext.Appointments
                .Where(a => a.AppointmentId == request.AppointmentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (appointment == null)
            {
                return result;
            }

            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
