using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<object>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<object>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var user = await _userRepository.GetByIdAsync(request.UserId);

            result.Value = new
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Invoices = user.Invoices.Select(invoice => new
                {
                    InvoiceId = invoice.InvoiceId,
                    IssuedAt = invoice.IssuedAt,
                    IsPaid = invoice.IsPaid
                })
            };

            return result;
        }
    }
}