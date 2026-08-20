using EmployeeManagement.Application.Attendances.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendanceById;

public class GetAttendanceByIdQuery
    : IRequest<AttendanceDto?>
{
    public Guid AttendanceId { get; set; }

    public GetAttendanceByIdQuery(Guid attendanceId)
    {
        AttendanceId = attendanceId;
    }
}