using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommand : IRequest<OperationResult>
    {
        public int UserId { get; set; }
    }
}
