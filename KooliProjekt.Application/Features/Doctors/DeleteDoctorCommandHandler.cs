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


namespace KooliProjekt.Application.Features.Doctors
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDoctorCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Doctors
                .Where(d => d.DoctorId == request.DoctorId)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
