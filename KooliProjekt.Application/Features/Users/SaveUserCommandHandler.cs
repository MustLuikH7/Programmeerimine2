using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public SaveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                user = new User();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.Phone = request.Phone;
            
            await _userRepository.SaveAsync(user);

            return result;
        }
    }
}