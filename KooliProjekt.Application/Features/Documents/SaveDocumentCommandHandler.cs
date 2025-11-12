using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Documents
{
    public class SaveDocumentCommandHandler : IRequestHandler<SaveDocumentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDocumentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var document = new Document();
            if(request.DocumentId == 0)
            {
                await _dbContext.Documents.AddAsync(document, cancellationToken);
            }
            else
            {
                document = await _dbContext.Documents.FindAsync(new object[] { request.DocumentId }, cancellationToken);
            }

        

            document.AppointmentId = request.AppointmentId;
            document.DoctorId = request.DoctorId;
            document.FileName = request.FileName;
            document.FilePath = request.FilePath;
            document.UploadedAt = request.UploadedAt;
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}