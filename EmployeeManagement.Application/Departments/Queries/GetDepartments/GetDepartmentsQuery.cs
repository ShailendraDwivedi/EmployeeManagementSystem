using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Departments.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Departments.Queries.GetDepartments
{
    public class GetDepartmentsQuery : IRequest<PagedResult<DepartmentDto>>
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }
    }
}
