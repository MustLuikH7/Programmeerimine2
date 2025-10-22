using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class InvoiceItemQueryHandler : IRequestHandler<InvoiceItemsQuery, OperationResult<IList<InvoiceItem>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public InvoiceItemQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<InvoiceItem>>> Handle(InvoiceItemsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<InvoiceItem>>();
            result.Value = await _dbContext
                .InvoiceItems
                .OrderBy(list => list.InvoiceId)
                .ToListAsync();

            return result;
        }
    }
}
