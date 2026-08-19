using EmployeeManagement.Application.Employees.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public string EmployeeCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime DOB { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid DesignationId { get; set; }

    public DateTime JoiningDate { get; set; }
}