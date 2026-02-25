using System;
using System.Linq;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.DoctorId <= 0)
            {
                return result;
            }

            var doctor = await _dbContext.Doctors
                .Where(d => d.DoctorId == request.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);

            if (doctor == null)
            {
                return result;
            }

            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
