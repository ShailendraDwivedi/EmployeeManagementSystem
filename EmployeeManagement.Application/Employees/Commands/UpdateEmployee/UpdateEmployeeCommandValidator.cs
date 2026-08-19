using FluentValidation;

namespace EmployeeManagement.Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator
    : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Employee ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^[0-9]{10}$")
            .WithMessage(
                "Phone number must contain 10 digits.");

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department is required.");

        RuleFor(x => x.DesignationId)
            .NotEmpty()
            .WithMessage("Designation is required.");

        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .WithMessage(
                "Salary must be greater than 0.");

        RuleFor(x => x.JoiningDate)
            .NotEmpty()
            .WithMessage(
                "Joining date is required.");
    }
}