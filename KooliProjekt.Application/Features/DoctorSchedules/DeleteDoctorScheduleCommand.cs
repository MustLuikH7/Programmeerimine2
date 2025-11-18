using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class DeleteDoctorScheduleCommand : IRequest<OperationResult>
    {
        public int ScheduleId { get; set; }
    }
}
