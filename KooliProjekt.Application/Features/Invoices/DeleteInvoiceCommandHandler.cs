using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.InvoiceId <= 0)
            {
                return result;
            }

            var invoice = await _dbContext.Invoices
                .Where(i => i.InvoiceId == request.InvoiceId)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice == null)
            {
                return result;
            }

            _dbContext.Invoices.Remove(invoice);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
