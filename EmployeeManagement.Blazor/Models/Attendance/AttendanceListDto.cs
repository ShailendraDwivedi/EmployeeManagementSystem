namespace EmployeeManagement.Blazor.Models.Attendance
{
    public class AttendanceListDto
    {
        public Guid AttendanceId { get; set; }

        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public decimal? WorkingHours { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
