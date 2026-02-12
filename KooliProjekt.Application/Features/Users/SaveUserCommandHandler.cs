using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.UserId < 0)
            {
                result.AddPropertyError(nameof(request.UserId), "Id cannot be negative");
                return result;
            }

            var user = new User();

            if (request.UserId == 0)
            {
                await _dbContext.Users.AddAsync(user, cancellationToken);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
                if (user == null)
                {
                    result.AddError("Cannot find user with id " + request.UserId);
                    return result;
                }
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.Phone = request.Phone;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}