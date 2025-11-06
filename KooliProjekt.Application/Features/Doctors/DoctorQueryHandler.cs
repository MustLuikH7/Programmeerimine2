using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class DoctorsQueryHandler : IRequestHandler<DoctorsQuery, OperationResult<PagedResult<Doctor>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public DoctorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Doctor>>> Handle(DoctorsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Doctor>>();

            var paged = await _dbContext
                .Doctors
                .AsNoTracking()
                .OrderBy(d => d.FirstName)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
