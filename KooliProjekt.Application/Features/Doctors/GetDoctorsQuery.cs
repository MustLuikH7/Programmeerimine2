using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQuery : IRequest<OperationResult<DoctorDetailsDto>>
    {
        public int DoctorId { get; set; }
    }
}