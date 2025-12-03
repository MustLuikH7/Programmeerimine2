using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommandValidator : AbstractValidator<SaveDoctorCommand>
    {
        public SaveDoctorCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(256).WithMessage("Password hash cannot exceed 256 characters");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required")
                .MaximumLength(100).WithMessage("Specialty cannot exceed 100 characters");
        }
    }
}
