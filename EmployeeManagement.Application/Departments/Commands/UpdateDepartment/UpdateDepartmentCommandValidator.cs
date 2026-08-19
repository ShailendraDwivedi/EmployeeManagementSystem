using FluentValidation;

namespace EmployeeManagement.Application.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(
                "Department ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(
                "Department name is required.")

            .MinimumLength(2)
            .WithMessage(
                "Department name must contain at least 2 characters.")

            .MaximumLength(100)
            .WithMessage(
                "Department name cannot exceed 100 characters.");
    }
}