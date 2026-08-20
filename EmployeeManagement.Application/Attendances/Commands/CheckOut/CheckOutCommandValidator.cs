using FluentValidation;

namespace EmployeeManagement.Application.Attendances.Commands.CheckOut;

public class CheckOutCommandValidator
    : AbstractValidator<CheckOutCommand>
{
    public CheckOutCommandValidator()
    {
        RuleFor(x => x.AttendanceId)
            .NotEmpty()
            .WithMessage("Attendance ID is required.");
    }
}