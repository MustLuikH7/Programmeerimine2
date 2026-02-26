using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class UsersQuery : IRequest<OperationResult<PagedResult<UserDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}