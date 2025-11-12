using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class GetInvoiceItemQueryHandler : IRequestHandler<GetInvoiceItemQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .InvoiceItems
                .Where(item => item.ItemId == request.ItemId)
                .Select(item => new 
                {
                    ItemId = item.ItemId,
                    InvoiceId = item.InvoiceId,
                    Description = item.Description,
                    Amount = item.Amount
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}