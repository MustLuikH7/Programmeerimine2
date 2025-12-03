using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
    {
        public SaveInvoiceCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor is required");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User is required");

            RuleFor(x => x.IssuedAt)
                .NotEmpty().WithMessage("Issue date is required");
        }
    }
}
