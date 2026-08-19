using FluentValidation;

namespace EmployeeManagement.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator
    : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {

        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .WithMessage("Employee code is required.")
            .MaximumLength(20)
            .WithMessage("Employee code cannot exceed 20 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.")
            .MaximumLength(200);

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
            .WithMessage("Salary must be greater than 0.");

        RuleFor(x => x.JoiningDate)
            .NotEmpty()
            .WithMessage("Joining date is required.");
    }
}