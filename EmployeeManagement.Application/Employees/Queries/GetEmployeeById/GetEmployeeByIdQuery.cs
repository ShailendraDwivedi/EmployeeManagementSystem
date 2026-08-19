using EmployeeManagement.Application.Employees.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery : IRequest<EmployeeDto?>
{
    public Guid Id { get; set; }

    public GetEmployeeByIdQuery(Guid id)
    {
        Id = id;
    }
}