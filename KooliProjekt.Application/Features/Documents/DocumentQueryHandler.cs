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

namespace KooliProjekt.Application.Features.Documents
{
    public class DocumentQueryHandler : IRequestHandler<DocumentsQuery, OperationResult<PagedResult<DocumentDto>>>
    {
        public const int MaxPageSize = 100;
        private readonly ApplicationDbContext _dbContext;

        public DocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DocumentDto>>> Handle(DocumentsQuery request, CancellationToken cancellationToken)
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

            var result = new OperationResult<PagedResult<DocumentDto>>();

            var query = _dbContext.Documents.AsQueryable();

            if (!string.IsNullOrEmpty(request.FileName))
            {
                query = query.Where(d => d.FileName.Contains(request.FileName));
            }

            if (request.DoctorId.HasValue)
            {
                query = query.Where(d => d.DoctorId == request.DoctorId.Value);
            }

            result.Value = await query
                .OrderBy(d => d.DocumentId)
                .Select(d => new DocumentDto
                {
                    DocumentId = d.DocumentId,
                    AppointmentId = d.AppointmentId,
                    DoctorId = d.DoctorId,
                    FileName = d.FileName,
                    FilePath = d.FilePath,
                    UploadedAt = d.UploadedAt
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
