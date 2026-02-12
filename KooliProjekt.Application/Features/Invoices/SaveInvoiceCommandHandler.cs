using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();
            if (request.InvoiceId < 0)
            {
                result.AddPropertyError(nameof(request.InvoiceId), "Id cannot be negative");
                return result;
            }

            var invoice = new Invoice();

            if (request.InvoiceId == 0)
            {
                await _dbContext.Invoices.AddAsync(invoice, cancellationToken);
            }
            else
            {
                invoice = await _dbContext.Invoices.FindAsync(new object[] { request.InvoiceId }, cancellationToken);
                if (invoice == null)
                {
                    result.AddError("Cannot find invoice with id " + request.InvoiceId);
                    return result;
                }
            }

            invoice.AppointmentId = request.AppointmentId;
            invoice.DoctorId = request.DoctorId;
            invoice.UserId = request.UserId;
            invoice.IssuedAt = request.IssuedAt;
            invoice.IsPaid = request.IsPaid;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}