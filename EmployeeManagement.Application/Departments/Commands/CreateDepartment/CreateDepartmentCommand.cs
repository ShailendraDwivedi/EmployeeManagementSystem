using EmployeeManagement.Application.Departments.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommand : IRequest<DepartmentDto>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDeleted { get; set; } = true;
}