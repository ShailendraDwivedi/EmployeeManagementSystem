using FluentValidation;

namespace EmployeeManagement.Application.Attendances.Commands.CheckIn;

public class CheckInCommandValidator
    : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee is required.");
    }
}