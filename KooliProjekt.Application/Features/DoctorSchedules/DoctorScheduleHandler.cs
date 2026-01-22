using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class DoctorScheduleHandler : IRequestHandler<DoctorSchedulesQuery, OperationResult<PagedResult<DoctorScheduleDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public DoctorScheduleHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DoctorScheduleDto>>> Handle(DoctorSchedulesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<DoctorScheduleDto>>();
            result.Value = await _dbContext
                .DoctorSchedules
                .OrderBy(s => s.DoctorId)
                .Select(s => new DoctorScheduleDto
                {
                    ScheduleId = s.ScheduleId,
                    DoctorId = s.DoctorId,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo
                })
                .GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}
