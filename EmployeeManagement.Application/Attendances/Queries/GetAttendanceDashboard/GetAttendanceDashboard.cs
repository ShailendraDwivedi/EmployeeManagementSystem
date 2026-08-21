using EmployeeManagement.Application.Attendances.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendanceDashboard;

public class GetAttendanceDashboardQuery : IRequest<AttendanceDashboardDto>
{
    public int Year { get; set; }
    public int Month { get; set; }

    public GetAttendanceDashboardQuery(int year, int month)
    {
        Year = year;
        Month = month;
    }
}