using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class DocumentsQuery : IRequest<OperationResult<PagedResult<DocumentDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string FileName { get; set; }
        public int? DoctorId { get; set; }
    }
}