using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class DeleteInvoiceItemCommandHandler : IRequestHandler<DeleteInvoiceItemCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteInvoiceItemCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteInvoiceItemCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.ItemId <= 0)
            {
                return result;
            }

            var invoiceItem = await _dbContext.InvoiceItems
                .Where(i => i.ItemId == request.ItemId)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoiceItem == null)
            {
                return result;
            }

            _dbContext.InvoiceItems.Remove(invoiceItem);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
