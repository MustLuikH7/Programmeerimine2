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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoice = new Invoice();
            if(request.InvoiceId == 0)
            {
                await _dbContext.Invoices.AddAsync(invoice, cancellationToken);
            }
            else
            {
                invoice = await _dbContext.Invoices.FindAsync(new object[] { request.InvoiceId }, cancellationToken);
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