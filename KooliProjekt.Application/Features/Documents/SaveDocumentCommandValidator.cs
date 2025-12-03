using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Documents
{
    public class SaveDocumentCommandValidator : AbstractValidator<SaveDocumentCommand>
    {
        public SaveDocumentCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor is required");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File name is required")
                .MaximumLength(255).WithMessage("File name cannot exceed 255 characters");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("File path is required")
                .MaximumLength(1024).WithMessage("File path cannot exceed 1024 characters");

            RuleFor(x => x.UploadedAt)
                .NotEmpty().WithMessage("Upload date is required");
        }
    }
}
