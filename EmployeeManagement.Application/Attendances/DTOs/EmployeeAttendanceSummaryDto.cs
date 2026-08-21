namespace EmployeeManagement.Application.Attendances.DTOs;

public class EmployeeAttendanceSummaryDto
{
    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int PresentCount { get; set; }

    public int AbsentCount { get; set; }

    public int LateCount { get; set; }

    public int LeaveCount { get; set; }

    public decimal PresentPercentage { get; set; }

    public decimal TotalWorkingHours { get; set; }

    public decimal AverageWorkingHours { get; set; }
}