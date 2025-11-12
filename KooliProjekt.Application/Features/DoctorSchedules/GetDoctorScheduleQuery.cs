using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class GetDoctorScheduleQuery : IRequest<OperationResult<object>>
    {
        public int ScheduleId { get; set; }
    }
}