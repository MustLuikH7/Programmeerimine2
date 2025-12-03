using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public SaveInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);

            if (invoice == null)
            {
                invoice = new Invoice();
            }

            invoice.AppointmentId = request.AppointmentId;
            invoice.DoctorId = request.DoctorId;
            invoice.UserId = request.UserId;
            invoice.IssuedAt = request.IssuedAt;
            invoice.IsPaid = request.IsPaid;

            await _invoiceRepository.SaveAsync(invoice);

            return result;
        }
    }
}