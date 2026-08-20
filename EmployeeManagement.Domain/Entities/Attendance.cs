using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities
{
    public class Attendance
    {
        public Guid AttendanceId { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public decimal? WorkingHours { get; set; }

        // Navigation property
        public Employee Employee { get; set; } = null!;
    }
}
