using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class InvoiceQueryHandler : IRequestHandler<InvoicesQuery, OperationResult<PagedResult<InvoiceDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public InvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceDto>>> Handle(InvoicesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<InvoiceDto>>();
            result.Value = await _dbContext
                .Invoices
                .OrderBy(i => i.InvoiceId)
                .Select(i => new InvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    AppointmentId = i.AppointmentId,
                    DoctorId = i.DoctorId,
                    UserId = i.UserId,
                    IssuedAt = i.IssuedAt,
                    IsPaid = i.IsPaid
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
