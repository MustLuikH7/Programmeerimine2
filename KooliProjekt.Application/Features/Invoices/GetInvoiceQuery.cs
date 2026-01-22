using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQuery : IRequest<OperationResult<InvoiceDetailsDto>>
    {
        public int InvoiceId { get; set; }
    }
}