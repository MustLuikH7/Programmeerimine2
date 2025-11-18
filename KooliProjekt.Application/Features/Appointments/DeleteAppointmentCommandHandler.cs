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


namespace KooliProjekt.Application.Features.Appointments
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAppointmentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Appointments
                .Where(a => a.AppointmentId == request.AppointmentId)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
