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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var doctorSchedule = new DoctorSchedule();
            if(request.ScheduleId == 0)
            {
                await _dbContext.DoctorSchedules.AddAsync(doctorSchedule, cancellationToken);
            }
            else
            {
                doctorSchedule = await _dbContext.DoctorSchedules.FindAsync(new object[] { request.ScheduleId }, cancellationToken);
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