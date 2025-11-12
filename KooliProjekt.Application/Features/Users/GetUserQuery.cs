using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
   
    public class GetUserQuery : IRequest<OperationResult<object>>
    {
        public int UserId { get; set; }
        public int PageSize { get; set; }

    }
}