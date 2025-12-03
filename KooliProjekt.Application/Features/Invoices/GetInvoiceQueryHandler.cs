using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<object>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);

            result.Value = new
            {
                InvoiceId = invoice.InvoiceId,
                AppointmentId = invoice.AppointmentId,
                DoctorId = invoice.DoctorId,
                UserId = invoice.UserId,
                IssuedAt = invoice.IssuedAt,
                IsPaid = invoice.IsPaid,
                Appointment = new
                {
                    AppointmentTime = invoice.Appointment.AppointmentTime,
                    Status = invoice.Appointment.Status
                },
                Doctor = new
                {
                    FirstName = invoice.Doctor.FirstName,
                    LastName = invoice.Doctor.LastName
                },
                User = new
                {
                    FirstName = invoice.User.FirstName,
                    LastName = invoice.User.LastName,
                    Email = invoice.User.Email
                },
                InvoiceItems = invoice.InvoiceItems.Select(item => new
                {
                    ItemId = item.ItemId,
                    Description = item.Description,
                    Amount = item.Amount
                })
            };

            return result;
        }
    }
}