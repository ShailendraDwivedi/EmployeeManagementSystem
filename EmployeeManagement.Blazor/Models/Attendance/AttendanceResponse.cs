namespace EmployeeManagement.Blazor.Models.Attendance;

public class AttendanceResponse
{
    public Guid AttendanceId { get; set; }

    public string Message { get; set; } = string.Empty;
}