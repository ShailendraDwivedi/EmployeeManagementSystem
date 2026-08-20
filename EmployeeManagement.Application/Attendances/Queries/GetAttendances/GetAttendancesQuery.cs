using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Models;
using MediatR;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendances;

public class GetAttendancesQuery : IRequest<PagedResult<AttendanceListDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public string? Search { get; }
    public Guid? EmployeeId { get; }
    public DateTime? FromDate { get; }
    public DateTime? ToDate { get; }
    public string? Status { get; }

    public GetAttendancesQuery(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = null,
        Guid? employeeId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? status = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Search = search;
        EmployeeId = employeeId;
        FromDate = fromDate;
        ToDate = toDate;
        Status = status;
    }
}