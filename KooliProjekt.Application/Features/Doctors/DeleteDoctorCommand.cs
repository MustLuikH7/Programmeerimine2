using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class DeleteDoctorCommand : IRequest<OperationResult>
    {
        public int DoctorId { get; set; }
    }
}
