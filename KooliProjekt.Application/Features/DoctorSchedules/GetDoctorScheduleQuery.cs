using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class GetDoctorScheduleQuery : IRequest<OperationResult<DoctorScheduleDetailsDto>>
    {
        public int ScheduleId { get; set; }
    }
}