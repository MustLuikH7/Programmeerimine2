using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class DeleteAppointmentCommand : IRequest<OperationResult>
    {
        public int AppointmentId { get; set; }
    }
}
