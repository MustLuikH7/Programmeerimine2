using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandValidator : AbstractValidator<SaveUserCommand>
    {
        public SaveUserCommandValidator(ApplicationDbContext context)
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
        }
    }
}