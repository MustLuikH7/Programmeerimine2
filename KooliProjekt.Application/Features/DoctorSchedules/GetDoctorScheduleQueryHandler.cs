using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class GetDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, OperationResult<object>>
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;

        public GetDoctorScheduleQueryHandler(IDoctorScheduleRepository doctorScheduleRepository)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var schedule = await _doctorScheduleRepository.GetByIdAsync(request.ScheduleId);

            result.Value = new
            {
                ScheduleId = schedule.ScheduleId,
                DoctorId = schedule.DoctorId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                ValidFrom = schedule.ValidFrom,
                ValidTo = schedule.ValidTo,
                Doctor = new
                {
                    FirstName = schedule.Doctor.FirstName,
                    LastName = schedule.Doctor.LastName,
                    Specialty = schedule.Doctor.Specialty
                }
            };

            return result;
        }
    }
}