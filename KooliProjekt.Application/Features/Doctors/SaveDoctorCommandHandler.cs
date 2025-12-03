using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommandHandler : IRequestHandler<SaveDoctorCommand, OperationResult>
    {
        private readonly IDoctorRepository _doctorRepository;

        public SaveDoctorCommandHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<OperationResult> Handle(SaveDoctorCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);

            if (doctor == null)
            {
                doctor = new Doctor();
            }

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.Email = request.Email;
            doctor.PasswordHash = request.PasswordHash;
            doctor.Specialty = request.Specialty;

            await _doctorRepository.SaveAsync(doctor);

            return result;
        }
    }
}