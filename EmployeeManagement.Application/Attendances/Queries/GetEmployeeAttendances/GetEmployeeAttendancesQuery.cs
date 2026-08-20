using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Models;
using MediatR;

namespace EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendances;

public class GetEmployeeAttendancesQuery : IRequest<PagedResult<AttendanceListDto>>
{
    public Guid EmployeeId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}