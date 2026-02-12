using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommandValidator : AbstractValidator<SaveAppointmentCommand>
    {
        public SaveAppointmentCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor is required");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User is required");

            RuleFor(x => x.AppointmentTime)
                .NotEmpty().WithMessage("Appointment time is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("Created at is required");
        }
    }
}