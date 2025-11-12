using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQueryHandler : IRequestHandler<GetDoctorQuery, OperationResult<object>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            result.Value = await _dbContext
                .Doctors
                .Include(d => d.Schedules)
                .Include(d => d.Appointments)
                .Include(d => d.Invoices)
                .Include(d => d.Documents)
                .Where(d => d.DoctorId == request.DoctorId)
                .Select(d => new 
                {
                    d.DoctorId,
                    d.FirstName,
                    d.LastName,
                    d.Email,
                    d.Specialty,
                    Schedules = d.Schedules.Select(s => new 
                    {
                        s.ScheduleId,
                        s.DayOfWeek,
                        s.StartTime,
                        s.EndTime,
                        s.ValidFrom,
                        s.ValidTo
                    }),
                    Appointments = d.Appointments.Select(a => new
                    {
                        a.AppointmentId,
                        a.UserId,
                        a.Status,
                    }),
                    Invoices = d.Invoices.Select(i => new
                    {
                        i.InvoiceId,
                        i.UserId,
                        i.IssuedAt,
                        i.IsPaid
                    }),
                    Documents = d.Documents.Select(doc => new
                    {
                        doc.DocumentId,
                        doc.AppointmentId,
                        doc.FileName,
                        doc.UploadedAt
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}