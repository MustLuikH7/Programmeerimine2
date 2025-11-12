using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class GetDocumentQuery : IRequest<OperationResult<object>>
    {
        public int DocumentId { get; set; }
    }
}