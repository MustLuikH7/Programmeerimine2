using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQuery : IRequest<OperationResult<object>>
    {
        public int DoctorId { get; set; }
    }
}