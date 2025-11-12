using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class SaveInvoiceItemCommand : IRequest<OperationResult>
    {
        public int ItemId { get; set; }

        public int InvoiceId { get; set; }

      
        public string Description { get; set; }

      
        public int Amount { get; set; }
    }
}