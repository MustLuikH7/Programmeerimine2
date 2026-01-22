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
        private readonly ApplicationDbContext _dbContext;
        public DocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DocumentDto>>> Handle(DocumentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<DocumentDto>>();
            result.Value = await _dbContext
                .Documents
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
