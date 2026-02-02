using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace KooliProjekt.Application.Features.Documents
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDocumentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.DocumentId <= 0)
            {
                return result;
            }

            var document = await _dbContext.Documents
                .Where(d => d.DocumentId == request.DocumentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (document == null)
            {
                return result;
            }

            _dbContext.Documents.Remove(document);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
