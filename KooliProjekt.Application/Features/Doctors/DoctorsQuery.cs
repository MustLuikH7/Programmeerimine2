using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class DoctorsQuery : IRequest<OperationResult<PagedResult<DoctorDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}