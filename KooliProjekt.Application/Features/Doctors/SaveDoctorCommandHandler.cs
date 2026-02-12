using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommandHandler : IRequestHandler<SaveDoctorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDoctorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDoctorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.DoctorId < 0)
            {
                result.AddPropertyError(nameof(request.DoctorId), "Id cannot be negative");
                return result;
            }

            var doctor = new Doctor();

            if (request.DoctorId == 0)
            {
                await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
            }
            else
            {
                doctor = await _dbContext.Doctors.FindAsync(new object[] { request.DoctorId }, cancellationToken);
                if (doctor == null)
                {
                    result.AddError("Cannot find doctor with id " + request.DoctorId);
                    return result;
                }
            }

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.Email = request.Email;
            doctor.PasswordHash = request.PasswordHash;
            doctor.Specialty = request.Specialty;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}