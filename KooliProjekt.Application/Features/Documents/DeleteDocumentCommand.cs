using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class DeleteDocumentCommand : IRequest<OperationResult>
    {
        public int DocumentId { get; set; }
    }
}
