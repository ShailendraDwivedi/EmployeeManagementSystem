namespace EmployeeManagement.Blazor.Models.Attendance;

public class AttendanceDashboardDto
{
    public int TotalRecords { get; set; }

    public int PresentCount { get; set; }

    public int AbsentCount { get; set; }

    public int LateCount { get; set; }

    public int LeaveCount { get; set; }

    public decimal PresentPercentage { get; set; }

    public decimal TotalWorkingHours { get; set; }

    public decimal AverageWorkingHours { get; set; }

    public List<AttendanceMonthlySummaryDto> MonthlySummary { get; set; }
        = new();
}