using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class UserQueryHandler : IRequestHandler<UsersQuery, OperationResult<PagedResult<User>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public UserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<User>>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<User>>();
            result.Value = await _dbContext
                .Users
                .OrderBy(list => list.FirstName)
                .GetPagedAsync(request.Page, request.PageSize); 

            return result;
        }
    }
}
