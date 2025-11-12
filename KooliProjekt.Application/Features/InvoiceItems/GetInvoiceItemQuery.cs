using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class GetInvoiceItemQuery : IRequest<OperationResult<object>>
    {
        public int ItemId { get; set; }
    }
}