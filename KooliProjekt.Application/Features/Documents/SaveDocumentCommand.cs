using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.Documents
{
    public class SaveDocumentCommand : IRequest<OperationResult>
    {
        public int DocumentId { get; set; }

        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}