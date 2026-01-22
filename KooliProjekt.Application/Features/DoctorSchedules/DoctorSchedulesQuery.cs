using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class DoctorSchedulesQuery : IRequest<OperationResult<PagedResult<DoctorScheduleDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}