using FluentValidation;

namespace EmployeeManagement.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Department name is required.")
            .MinimumLength(2)
                .WithMessage("Department name must contain at least 2 characters.")
            .MaximumLength(100)
                .WithMessage("Department name cannot exceed 100 characters.");
    }
}