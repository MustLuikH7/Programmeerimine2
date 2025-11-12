using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAppointmentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Appointments
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .Include(a => a.Invoice)
                .Include(a => a.Documents)
                .Where(a => a.AppointmentId == request.AppointmentId)
                .Select(a => new 
                {
                    a.AppointmentId,
                    a.DoctorId,
                    a.UserId,
                    a.AppointmentTime,
                    a.Status,
                    a.CreatedAt,
                    a.CancelledAt,
                    Doctor = new 
                    {
                        a.Doctor.FirstName,
                        a.Doctor.LastName,
                        a.Doctor.Specialty
                    },
                    User = new
                    {
                        a.User.FirstName,
                        a.User.LastName,
                        a.User.Email
                    },
                    Invoice = a.Invoice != null ? new 
                    {
                        a.Invoice.InvoiceId,
                        a.Invoice.IssuedAt,
                        a.Invoice.IsPaid
                    } : null,
                    Documents = a.Documents.Select(doc => new
                    {
                        doc.DocumentId,
                        doc.FileName,
                        doc.UploadedAt
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}