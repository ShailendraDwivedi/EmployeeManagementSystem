

namespace EmployeeManagement.Application.Attendances.DTOs
{
    public class AttendanceMonthlySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public int LeaveDays { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal AverageWorkingHours { get; set; }
    }
}
