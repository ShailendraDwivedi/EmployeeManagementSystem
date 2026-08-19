using MediatR;

namespace EmployeeManagement.Application.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand : IRequest<bool>

{
    public Guid Id { get; set; }
}
