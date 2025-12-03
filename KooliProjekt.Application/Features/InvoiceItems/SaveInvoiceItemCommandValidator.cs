using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.InvoiceItems
{
    public class SaveInvoiceItemCommandValidator : AbstractValidator<SaveInvoiceItemCommand>
    {
        public SaveInvoiceItemCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.InvoiceId)
                .GreaterThan(0).WithMessage("Invoice is required");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");
        }
    }
}
