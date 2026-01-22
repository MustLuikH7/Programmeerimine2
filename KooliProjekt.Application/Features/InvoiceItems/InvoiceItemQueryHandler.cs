using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class InvoiceItemQueryHandler : IRequestHandler<InvoiceItemsQuery, OperationResult<PagedResult<InvoiceItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public InvoiceItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceItemDto>>> Handle(InvoiceItemsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<InvoiceItemDto>>();
            result.Value = await _dbContext
                .InvoiceItems
                .OrderBy(i => i.InvoiceId)
                .Select(i => new InvoiceItemDto
                {
                    ItemId = i.ItemId,
                    InvoiceId = i.InvoiceId,
                    Description = i.Description,
                    Amount = i.Amount
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
