using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Designations.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Designations.Queries.GetDesignations
{
    public class GetDesignationsQuery : IRequest<PagedResult<DesignationDto>>
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }
    }
}
