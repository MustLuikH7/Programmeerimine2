using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class GetDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorScheduleQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .DoctorSchedules
                .Where(schedule => schedule.ScheduleId == request.ScheduleId)
                .Select(schedule => new 
                {
                    schedule.ScheduleId,
                    schedule.DoctorId,
                    schedule.DayOfWeek,
                    schedule.StartTime,
                    schedule.EndTime,
                    schedule.ValidFrom,
                    schedule.ValidTo
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}