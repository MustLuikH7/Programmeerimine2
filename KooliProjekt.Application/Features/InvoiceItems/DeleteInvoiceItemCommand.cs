using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class DeleteInvoiceItemCommand : IRequest<OperationResult>
    {
        public int ItemId { get; set; }
    }
}
