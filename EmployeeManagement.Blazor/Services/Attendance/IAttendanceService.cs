using EmployeeManagement.Blazor.Models;
using EmployeeManagement.Blazor.Models.Attendance;

namespace EmployeeManagement.Blazor.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<PagedResult<AttendanceDto>> GetAttendancesAsync(int pageNumber = 1, int pageSize = 10, string? search = null, Guid? employeeId = null, DateTime? fromDate = null, DateTime? toDate = null, string? status = null);

        Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id);

        Task<Guid?> CheckInAsync(Guid employeeId);

        Task<bool> CheckOutAsync(Guid attendanceId);

        Task<PagedResult<AttendanceDto>> GetEmployeeAttendancesAsync(
        Guid employeeId,
        int pageNumber = 1,
        int pageSize = 10);
    }
}