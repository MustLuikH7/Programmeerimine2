using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class GetInvoiceItemQuery : IRequest<OperationResult<InvoiceItemDetailsDto>>
    {
        public int ItemId { get; set; }
    }
}