using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class InvoiceItemsQuery : IRequest<OperationResult<PagedResult<InvoiceItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Description { get; set; }
        public int? InvoiceId { get; set; }
    }
}