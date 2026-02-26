using System;
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
        public const int MaxPageSize = 100;
        private readonly ApplicationDbContext _dbContext;

        public UserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<UserDto>>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero.", nameof(request));
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be greater than zero.", nameof(request));
            }

            if (request.PageSize > MaxPageSize)
            {
                throw new ArgumentException($"PageSize cannot be greater than {MaxPageSize}.", nameof(request));
            }

            var result = new OperationResult<PagedResult<UserDto>>();

            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(u => u.FirstName.Contains(request.Name) || u.LastName.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(u => u.Email.Contains(request.Email));
            }

            result.Value = await query
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
