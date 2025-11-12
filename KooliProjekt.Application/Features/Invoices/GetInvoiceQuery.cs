using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQuery : IRequest<OperationResult<object>>
    {
        public int InvoiceId { get; set; }
    }
}