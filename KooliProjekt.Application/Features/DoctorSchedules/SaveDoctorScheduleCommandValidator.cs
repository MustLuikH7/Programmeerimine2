using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.DoctorSchedules
{
    public class SaveDoctorScheduleCommandValidator : AbstractValidator<SaveDoctorScheduleCommand>
    {
        public SaveDoctorScheduleCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor is required");

            RuleFor(x => x.DayOfWeek)
                .NotEmpty().WithMessage("Day of week is required")
                .MaximumLength(10).WithMessage("Day of week cannot exceed 10 characters");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

            RuleFor(x => x.ValidFrom)
                .NotEmpty().WithMessage("Valid from date is required");
        }
    }
}