using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class SaveInvoiceItemCommandHandler : IRequestHandler<SaveInvoiceItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceItemCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.ItemId < 0)
            {
                result.AddPropertyError(nameof(request.ItemId), "Id cannot be negative");
                return result;
            }

            var invoiceItem = new InvoiceItem();

            if (request.ItemId == 0)
            {
                await _dbContext.InvoiceItems.AddAsync(invoiceItem, cancellationToken);
            }
            else
            {
                invoiceItem = await _dbContext.InvoiceItems.FindAsync(new object[] { request.ItemId }, cancellationToken);
                if (invoiceItem == null)
                {
                    result.AddError("Cannot find invoice item with id " + request.ItemId);
                    return result;
                }
            }

            invoiceItem.InvoiceId = request.InvoiceId;
            invoiceItem.Description = request.Description;
            invoiceItem.Amount = request.Amount;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}