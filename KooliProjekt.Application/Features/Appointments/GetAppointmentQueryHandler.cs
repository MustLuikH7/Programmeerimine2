using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, OperationResult<AppointmentDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAppointmentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<AppointmentDetailsDto>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AppointmentDetailsDto>();

            if (request == null)
            {
                return result;
            }

            result.Value = await _dbContext
                .Appointments
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .Include(a => a.Invoice)
                .Include(a => a.Documents)
                .Where(a => a.AppointmentId == request.AppointmentId)
                .Select(a => new AppointmentDetailsDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorId = a.DoctorId,
                    UserId = a.UserId,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt,
                    CancelledAt = a.CancelledAt,
                    Doctor = new DoctorDto
                    {
                        DoctorId = a.Doctor.DoctorId,
                        FirstName = a.Doctor.FirstName,
                        LastName = a.Doctor.LastName,
                        Email = a.Doctor.Email,
                        Specialty = a.Doctor.Specialty
                    },
                    User = new UserDto
                    {
                        UserId = a.User.UserId,
                        FirstName = a.User.FirstName,
                        LastName = a.User.LastName,
                        Email = a.User.Email,
                        Phone = a.User.Phone
                    },
                    Invoice = a.Invoice != null ? new InvoiceDto
                    {
                        InvoiceId = a.Invoice.InvoiceId,
                        AppointmentId = a.Invoice.AppointmentId,
                        DoctorId = a.Invoice.DoctorId,
                        UserId = a.Invoice.UserId,
                        IssuedAt = a.Invoice.IssuedAt,
                        IsPaid = a.Invoice.IsPaid
                    } : null,
                    Documents = a.Documents.Select(doc => new DocumentDto
                    {
                        DocumentId = doc.DocumentId,
                        AppointmentId = doc.AppointmentId,
                        DoctorId = doc.DoctorId,
                        FileName = doc.FileName,
                        FilePath = doc.FilePath,
                        UploadedAt = doc.UploadedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}