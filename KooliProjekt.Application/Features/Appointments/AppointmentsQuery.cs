using System;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class AppointmentsQuery : IRequest<OperationResult<PagedResult<AppointmentDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}