using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, OperationResult<object>>
    {
        private readonly IDocumentRepository _documentRepository;

        public GetDocumentQueryHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<OperationResult<object>> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var document = await _documentRepository.GetByIdAsync(request.DocumentId);

            result.Value = new
            {
                DocumentId = document.DocumentId,
                AppointmentId = document.AppointmentId,
                DoctorId = document.DoctorId,
                FileName = document.FileName,
                FilePath = document.FilePath,
                UploadedAt = document.UploadedAt,
                Appointment = new
                {
                    AppointmentTime = document.Appointment.AppointmentTime,
                    Status = document.Appointment.Status
                },
                Doctor = new
                {
                    FirstName = document.Doctor.FirstName,
                    LastName = document.Doctor.LastName,
                    Specialty = document.Doctor.Specialty
                }
            };

            return result;
        }
    }
}