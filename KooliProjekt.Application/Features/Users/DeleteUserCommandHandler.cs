using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUserCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.UserId <= 0)
            {
                return result;
            }

            var user = await _dbContext.Users
                .Where(u => u.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return result;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
