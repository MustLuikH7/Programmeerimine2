using System;
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
        public const int MaxPageSize = 100;
        private readonly ApplicationDbContext _dbContext;

        public DoctorScheduleHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DoctorScheduleDto>>> Handle(DoctorSchedulesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero.", nameof(request));
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be greater than zero.", nameof(request));
            }

            if (request.PageSize > MaxPageSize)
            {
                throw new ArgumentException($"PageSize cannot be greater than {MaxPageSize}.", nameof(request));
            }

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
