using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class SaveInvoiceItemCommandHandler : IRequestHandler<SaveInvoiceItemCommand, OperationResult>
    {
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        public SaveInvoiceItemCommandHandler(IInvoiceItemRepository invoiceItemRepository)
        {
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<OperationResult> Handle(SaveInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoiceItem = await _invoiceItemRepository.GetByIdAsync(request.ItemId);

            if (invoiceItem == null)
            {
                invoiceItem = new InvoiceItem();
            }

            invoiceItem.InvoiceId = request.InvoiceId;
            invoiceItem.Description = request.Description;
            invoiceItem.Amount = request.Amount;

            await _invoiceItemRepository.SaveAsync(invoiceItem);

            return result;
        }
    }
}