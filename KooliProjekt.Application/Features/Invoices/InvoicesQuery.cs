using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class InvoicesQuery : IRequest<OperationResult<PagedResult<InvoiceDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public bool? IsPaid { get; set; }
        public int? UserId { get; set; }
    }
}