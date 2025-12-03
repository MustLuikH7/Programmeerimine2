using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class SaveDoctorScheduleCommandHandler : IRequestHandler<SaveDoctorScheduleCommand, OperationResult>
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;

        public SaveDoctorScheduleCommandHandler(IDoctorScheduleRepository doctorScheduleRepository)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
        }

        public async Task<OperationResult> Handle(SaveDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var doctorSchedule = await _doctorScheduleRepository.GetByIdAsync(request.ScheduleId);

            if (doctorSchedule == null)
            {
                doctorSchedule = new DoctorSchedule();
            }

            doctorSchedule.DoctorId = request.DoctorId;
            doctorSchedule.DayOfWeek = request.DayOfWeek;
            doctorSchedule.StartTime = request.StartTime;
            doctorSchedule.EndTime = request.EndTime;
            doctorSchedule.ValidFrom = request.ValidFrom;
            doctorSchedule.ValidTo = request.ValidTo;

            await _doctorScheduleRepository.SaveAsync(doctorSchedule);

            return result;
        }
    }
}