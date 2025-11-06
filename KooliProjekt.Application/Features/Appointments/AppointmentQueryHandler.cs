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
    public class AppointmentQueryHandler : IRequestHandler<AppointmentsQuery, OperationResult<PagedResult<Appointment>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public AppointmentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Appointment>>> Handle(AppointmentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Appointment>>();
            result.Value = await _dbContext
                .Appointments
                .OrderBy(list => list.AppointmentId)
                .GetPagedAsync(request.Page, request.PageSize);
            return result;
        }
    }
}
