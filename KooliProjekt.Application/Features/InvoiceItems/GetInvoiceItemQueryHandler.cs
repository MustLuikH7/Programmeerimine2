using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class GetInvoiceItemQueryHandler : IRequestHandler<GetInvoiceItemQuery, OperationResult<InvoiceItemDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceItemDetailsDto>> Handle(GetInvoiceItemQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceItemDetailsDto>();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.ItemId <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .InvoiceItems
                .Include(i => i.Invoice)
                .Where(i => i.ItemId == request.ItemId)
                .Select(i => new InvoiceItemDetailsDto
                {
                    ItemId = i.ItemId,
                    InvoiceId = i.InvoiceId,
                    Description = i.Description,
                    Amount = i.Amount,
                    Invoice = new InvoiceDto
                    {
                        InvoiceId = i.Invoice.InvoiceId,
                        AppointmentId = i.Invoice.AppointmentId,
                        DoctorId = i.Invoice.DoctorId,
                        UserId = i.Invoice.UserId,
                        IssuedAt = i.Invoice.IssuedAt,
                        IsPaid = i.Invoice.IsPaid
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}