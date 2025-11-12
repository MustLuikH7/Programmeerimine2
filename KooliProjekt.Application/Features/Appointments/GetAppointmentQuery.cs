using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQuery : IRequest<OperationResult<object>>
    {
        public int AppointmentId { get; set; }
    }
}