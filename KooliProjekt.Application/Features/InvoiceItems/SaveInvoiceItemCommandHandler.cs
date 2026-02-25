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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoiceItem = new InvoiceItem();
            if(request.ItemId == 0)
            {
                await _dbContext.InvoiceItems.AddAsync(invoiceItem, cancellationToken);
            }
            else
            {
                invoiceItem = await _dbContext.InvoiceItems.FindAsync(new object[] { request.ItemId }, cancellationToken);
            }


            invoiceItem.InvoiceId = request.InvoiceId;
            invoiceItem.Description = request.Description;
            invoiceItem.Amount = request.Amount;
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}