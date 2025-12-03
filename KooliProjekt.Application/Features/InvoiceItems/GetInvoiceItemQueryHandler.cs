using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class GetInvoiceItemQueryHandler : IRequestHandler<GetInvoiceItemQuery, OperationResult<object>>
    {
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public GetInvoiceItemQueryHandler(IInvoiceItemRepository invoiceItemRepository)
        {
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var invoiceItem = await _invoiceItemRepository.GetByIdAsync(request.ItemId);

            result.Value = new
            {
                ItemId = invoiceItem.ItemId,
                InvoiceId = invoiceItem.InvoiceId,
                Description = invoiceItem.Description,
                Amount = invoiceItem.Amount,
                Invoice = new
                {
                    InvoiceId = invoiceItem.Invoice.InvoiceId,
                    IssuedAt = invoiceItem.Invoice.IssuedAt,
                    IsPaid = invoiceItem.Invoice.IsPaid
                }
            };

            return result;
        }
    }
}