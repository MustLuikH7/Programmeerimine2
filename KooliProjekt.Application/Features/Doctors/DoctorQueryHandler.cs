using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class DoctorsQueryHandler : IRequestHandler<DoctorsQuery, OperationResult<PagedResult<DoctorDto>>>
    {
        public const int MaxPageSize = 100;
        private readonly ApplicationDbContext _dbContext;

        public DoctorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DoctorDto>>> Handle(DoctorsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero.", nameof(request));
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be greater than zero.", nameof(request));
            }

            if (request.PageSize > MaxPageSize)
            {
                throw new ArgumentException($"PageSize cannot be greater than {MaxPageSize}.", nameof(request));
            }

            var result = new OperationResult<PagedResult<DoctorDto>>();

            result.Value = await _dbContext
                .Doctors
                .AsNoTracking()
                .OrderBy(d => d.FirstName)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Email = d.Email,
                    Specialty = d.Specialty
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
