using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Invoices
                .Include(invoice => invoice.InvoiceItems)
                .Where(invoice => invoice.InvoiceId == request.InvoiceId)
                .Select(invoice => new 
                {
                    invoice.InvoiceId,
                    invoice.AppointmentId,
                    invoice.DoctorId,
                    invoice.UserId,
                    invoice.IssuedAt,
                    invoice.IsPaid,
                    InvoiceItems = invoice.InvoiceItems.Select(item => new
                    {
                        item.ItemId,
                        item.Description,
                        item.Amount
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}