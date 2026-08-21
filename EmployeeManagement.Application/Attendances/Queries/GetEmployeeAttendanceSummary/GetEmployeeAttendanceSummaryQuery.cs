using EmployeeManagement.Application.Attendances.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendanceSummary;

public class GetEmployeeAttendanceSummaryQuery : IRequest<EmployeeAttendanceSummaryDto?>
{
    public Guid EmployeeId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public GetEmployeeAttendanceSummaryQuery(Guid employeeId, int year, int month)
    {
        EmployeeId = employeeId;
        Year = year;
        Month = month;
    }
}