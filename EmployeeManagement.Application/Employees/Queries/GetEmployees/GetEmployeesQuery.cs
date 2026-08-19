using EmployeeManagement.Application.Employees.DTOs;
using MediatR;
using EmployeeManagement.Application.Common.Models;

namespace EmployeeManagement.Application.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IRequest<PagedResult<EmployeeDto>>
{
    public string? Search { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? DesignationId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string? SortOrder { get; set; }
}