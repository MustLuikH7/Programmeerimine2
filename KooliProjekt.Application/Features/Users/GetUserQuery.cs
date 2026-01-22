using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{

    public class GetUserQuery : IRequest<OperationResult<UserDetailsDto>>
    {
        public int UserId { get; set; }
        public int PageSize { get; set; }

    }
}