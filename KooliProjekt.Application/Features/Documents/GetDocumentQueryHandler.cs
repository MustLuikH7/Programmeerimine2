using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Documents
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Documents
                .Where(doc => doc.DocumentId == request.DocumentId)
                .Select(doc => new 
                {
                    doc.DocumentId,
                    doc.AppointmentId,
                    doc.DoctorId,
                    doc.FileName,
                    doc.FilePath,
                    doc.UploadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}