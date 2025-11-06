using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class DoctorScheduleHandler : IRequestHandler<DoctorSchedulesQuery, OperationResult<PagedResult<DoctorSchedule>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public DoctorScheduleHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DoctorSchedule>>> Handle(DoctorSchedulesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<DoctorSchedule>>();
            result.Value = await _dbContext
                .DoctorSchedules
                .OrderBy(list => list.DoctorId)
                .GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}
