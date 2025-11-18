using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoiceCommand : IRequest<OperationResult>
    {
        public int InvoiceId { get; set; }
    }
}
