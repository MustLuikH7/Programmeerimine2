using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class UserQueryHandler : IRequestHandler<UsersQuery, OperationResult<PagedResult<UserDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public UserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<UserDto>>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<UserDto>>();
            result.Value = await _dbContext
                .Users
                .OrderBy(u => u.FirstName)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
