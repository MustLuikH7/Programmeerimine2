using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class DeleteDoctorScheduleCommandHandler : IRequestHandler<DeleteDoctorScheduleCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDoctorScheduleCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.ScheduleId <= 0)
            {
                return result;
            }

            var schedule = await _dbContext.DoctorSchedules
                .Where(ds => ds.ScheduleId == request.ScheduleId)
                .FirstOrDefaultAsync(cancellationToken);

            if (schedule == null)
            {
                return result;
            }

            _dbContext.DoctorSchedules.Remove(schedule);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
