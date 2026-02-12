using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class SaveDoctorScheduleCommandHandler : IRequestHandler<SaveDoctorScheduleCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDoctorScheduleCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.ScheduleId < 0)
            {
                result.AddPropertyError(nameof(request.ScheduleId), "Id cannot be negative");
                return result;
            }

            var doctorSchedule = new DoctorSchedule();

            if (request.ScheduleId == 0)
            {
                await _dbContext.DoctorSchedules.AddAsync(doctorSchedule, cancellationToken);
            }
            else
            {
                doctorSchedule = await _dbContext.DoctorSchedules.FindAsync(new object[] { request.ScheduleId }, cancellationToken);
                if (doctorSchedule == null)
                {
                    result.AddError("Cannot find schedule with id " + request.ScheduleId);
                    return result;
                }
            }

            doctorSchedule.DoctorId = request.DoctorId;
            doctorSchedule.DayOfWeek = request.DayOfWeek;
            doctorSchedule.StartTime = request.StartTime;
            doctorSchedule.EndTime = request.EndTime;
            doctorSchedule.ValidFrom = request.ValidFrom;
            doctorSchedule.ValidTo = request.ValidTo;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}