using FluentValidation;

namespace EmployeeManagement.Application.Designations.Commands.CreateDesignation;

public class CreateDesignationCommandValidator : AbstractValidator<CreateDesignationCommand>
{
    public CreateDesignationCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Designation name is required.")
            .MinimumLength(2)
                .WithMessage(
                    "Designation name must contain at least 2 characters.")
            .MaximumLength(100)
                .WithMessage(
                    "Designation name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(
                "Description cannot exceed 500 characters.");
    }
}