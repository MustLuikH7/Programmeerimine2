using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class SaveDocumentCommandHandler : IRequestHandler<SaveDocumentCommand, OperationResult>
    {
        private readonly IDocumentRepository _documentRepository;

        public SaveDocumentCommandHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<OperationResult> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var document = await _documentRepository.GetByIdAsync(request.DocumentId);

            if (document == null)
            {
                document = new Document();
            }

            document.AppointmentId = request.AppointmentId;
            document.DoctorId = request.DoctorId;
            document.FileName = request.FileName;
            document.FilePath = request.FilePath;
            document.UploadedAt = request.UploadedAt;

            await _documentRepository.SaveAsync(document);

            return result;
        }
    }
}