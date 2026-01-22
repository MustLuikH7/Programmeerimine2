using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<InvoiceDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceDetailsDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceDetailsDto>();

            if (request == null)
            {
                return result;
            }

            result.Value = await _dbContext
                .Invoices
                .Include(i => i.Appointment)
                .Include(i => i.Doctor)
                .Include(i => i.User)
                .Include(i => i.InvoiceItems)
                .Where(i => i.InvoiceId == request.InvoiceId)
                .Select(i => new InvoiceDetailsDto
                {
                    InvoiceId = i.InvoiceId,
                    AppointmentId = i.AppointmentId,
                    DoctorId = i.DoctorId,
                    UserId = i.UserId,
                    IssuedAt = i.IssuedAt,
                    IsPaid = i.IsPaid,
                    Appointment = new AppointmentDto
                    {
                        AppointmentId = i.Appointment.AppointmentId,
                        DoctorId = i.Appointment.DoctorId,
                        UserId = i.Appointment.UserId,
                        AppointmentTime = i.Appointment.AppointmentTime,
                        Status = i.Appointment.Status,
                        CreatedAt = i.Appointment.CreatedAt,
                        CancelledAt = i.Appointment.CancelledAt
                    },
                    Doctor = new DoctorDto
                    {
                        DoctorId = i.Doctor.DoctorId,
                        FirstName = i.Doctor.FirstName,
                        LastName = i.Doctor.LastName,
                        Email = i.Doctor.Email,
                        Specialty = i.Doctor.Specialty
                    },
                    User = new UserDto
                    {
                        UserId = i.User.UserId,
                        FirstName = i.User.FirstName,
                        LastName = i.User.LastName,
                        Email = i.User.Email,
                        Phone = i.User.Phone
                    },
                    InvoiceItems = i.InvoiceItems.Select(item => new InvoiceItemDto
                    {
                        ItemId = item.ItemId,
                        InvoiceId = item.InvoiceId,
                        Description = item.Description,
                        Amount = item.Amount
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}