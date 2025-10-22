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
    public class DoctorsQueryHandler : IRequestHandler<DoctorsQuery, OperationResult<IList<Doctor>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public DoctorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Doctor>>> Handle(DoctorsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Doctor>>();
            result.Value = await _dbContext
                .Doctors
                .OrderBy(list => list.FirstName)
                .ToListAsync();

            return result;
        }
    }
}
