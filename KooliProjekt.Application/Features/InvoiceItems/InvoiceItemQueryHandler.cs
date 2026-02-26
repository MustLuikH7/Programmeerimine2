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

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class InvoiceItemQueryHandler : IRequestHandler<InvoiceItemsQuery, OperationResult<PagedResult<InvoiceItemDto>>>
    {
        public const int MaxPageSize = 100;
        private readonly ApplicationDbContext _dbContext;

        public InvoiceItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceItemDto>>> Handle(InvoiceItemsQuery request, CancellationToken cancellationToken)
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

            var result = new OperationResult<PagedResult<InvoiceItemDto>>();

            var query = _dbContext.InvoiceItems.AsQueryable();

            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(i => i.Description.Contains(request.Description));
            }

            if (request.InvoiceId.HasValue)
            {
                query = query.Where(i => i.InvoiceId == request.InvoiceId.Value);
            }

            result.Value = await query
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
