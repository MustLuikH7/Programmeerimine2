using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class GetDocumentQuery : IRequest<OperationResult<DocumentDetailsDto>>
    {
        public int DocumentId { get; set; }
    }
}