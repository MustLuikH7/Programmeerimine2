using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Documents
                .Where(d => d.DocumentId == request.DocumentId)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
