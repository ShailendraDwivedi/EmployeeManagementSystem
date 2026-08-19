using FluentValidation;

namespace EmployeeManagement.Application.Designations.Commands.UpdateDesignation;

public class UpdateDesignationCommandValidator : AbstractValidator<UpdateDesignationCommand>
{
    public UpdateDesignationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(
                "Designation ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(
                "Designation name is required.")

            .MinimumLength(2)
            .WithMessage(
                "Designation name must contain at least 2 characters.")
            .MaximumLength(100)
            .WithMessage(
                "Designation name cannot exceed 100 characters.");
    }
}