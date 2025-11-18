using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .DoctorSchedules
                .Where(ds => ds.ScheduleId == request.ScheduleId)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
