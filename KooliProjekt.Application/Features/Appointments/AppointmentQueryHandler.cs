using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Appointments;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class AppointmentQueryHandler : IRequestHandler<AppointmentsQuery, OperationResult<PagedResult<AppointmentDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public AppointmentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<AppointmentDto>>> Handle(AppointmentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<AppointmentDto>>();
            result.Value = await _dbContext
                .Appointments
                .OrderBy(a => a.AppointmentId)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    UserId = a.UserId,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    CancelledAt = a.CancelledAt
                })
                .GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}
