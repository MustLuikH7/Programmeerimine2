using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQueryHandler : IRequestHandler<GetDoctorQuery, OperationResult<DoctorDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<DoctorDetailsDto>> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DoctorDetailsDto>();

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.DoctorId <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Doctors
                .Include(d => d.Schedules)
                .Include(d => d.Appointments)
                .Include(d => d.Invoices)
                .Include(d => d.Documents)
                .Where(d => d.DoctorId == request.DoctorId)
                .Select(d => new DoctorDetailsDto
                {
                    DoctorId = d.DoctorId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Email = d.Email,
                    Specialty = d.Specialty,
                    Schedules = d.Schedules.Select(s => new DoctorScheduleDto
                    {
                        ScheduleId = s.ScheduleId,
                        DoctorId = s.DoctorId,
                        DayOfWeek = s.DayOfWeek,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList(),
                    Appointments = d.Appointments.Select(a => new AppointmentDto
                    {
                        AppointmentId = a.AppointmentId,
                        DoctorId = a.DoctorId,
                        UserId = a.UserId,
                        AppointmentTime = a.AppointmentTime,
                        Status = a.Status,
                        CreatedAt = a.CreatedAt,
                        CancelledAt = a.CancelledAt
                    }).ToList(),
                    Invoices = d.Invoices.Select(i => new InvoiceDto
                    {
                        InvoiceId = i.InvoiceId,
                        AppointmentId = i.AppointmentId,
                        DoctorId = i.DoctorId,
                        UserId = i.UserId,
                        IssuedAt = i.IssuedAt,
                        IsPaid = i.IsPaid
                    }).ToList(),
                    Documents = d.Documents.Select(doc => new DocumentDto
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