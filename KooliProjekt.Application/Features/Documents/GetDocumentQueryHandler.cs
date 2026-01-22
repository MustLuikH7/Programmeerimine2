using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Documents
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, OperationResult<DocumentDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<DocumentDetailsDto>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DocumentDetailsDto>();

            if (request == null)
            {
                return result;
            }

            result.Value = await _dbContext
                .Documents
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .Where(d => d.DocumentId == request.DocumentId)
                .Select(d => new DocumentDetailsDto
                {
                    DocumentId = d.DocumentId,
                    AppointmentId = d.AppointmentId,
                    DoctorId = d.DoctorId,
                    FileName = d.FileName,
                    FilePath = d.FilePath,
                    UploadedAt = d.UploadedAt,
                    Appointment = new AppointmentDto
                    {
                        AppointmentId = d.Appointment.AppointmentId,
                        DoctorId = d.Appointment.DoctorId,
                        UserId = d.Appointment.UserId,
                        AppointmentTime = d.Appointment.AppointmentTime,
                        Status = d.Appointment.Status,
                        CreatedAt = d.Appointment.CreatedAt,
                        CancelledAt = d.Appointment.CancelledAt
                    },
                    Doctor = new DoctorDto
                    {
                        DoctorId = d.Doctor.DoctorId,
                        FirstName = d.Doctor.FirstName,
                        LastName = d.Doctor.LastName,
                        Email = d.Doctor.Email,
                        Specialty = d.Doctor.Specialty
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}