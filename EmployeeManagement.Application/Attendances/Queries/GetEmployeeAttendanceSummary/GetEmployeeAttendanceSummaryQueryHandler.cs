using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendanceSummary;

public class GetEmployeeAttendanceSummaryQueryHandler : IRequestHandler<GetEmployeeAttendanceSummaryQuery, EmployeeAttendanceSummaryDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetEmployeeAttendanceSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeAttendanceSummaryDto?> Handle(GetEmployeeAttendanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(request.Year, request.Month, 1);

        var endDate = startDate.AddMonths(1);

        var employee = await _unitOfWork.Employees.Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.EmployeeId,
                cancellationToken);

        if (employee == null)
            return null;

        var records = await _unitOfWork.Attendances.Query()
            .AsNoTracking()
            .Where(x =>
                x.EmployeeId == request.EmployeeId &&
                x.AttendanceDate.HasValue &&
                x.AttendanceDate.Value >= startDate &&
                x.AttendanceDate.Value < endDate)
            .ToListAsync(cancellationToken);

        var totalRecords = records.Count;

        var presentCount = records.Count(x =>
            x.Status == AttendanceStatus.Present);

        var absentCount = records.Count(x =>
            x.Status == AttendanceStatus.Absent);

        var lateCount = records.Count(x =>
            x.Status == AttendanceStatus.Late);

        var leaveCount = records.Count(x =>
            x.Status == AttendanceStatus.Leave);

        var workingRecords = records
            .Where(x =>
                x.CheckIn != default &&
                x.CheckOut != default)
            .ToList();

        decimal totalWorkingHours = workingRecords.Sum(x => (decimal)(x.CheckOut! - x.CheckIn!).Value.TotalHours);

        decimal averageWorkingHours = workingRecords.Count == 0 ? 0 : totalWorkingHours / workingRecords.Count;

        decimal presentPercentage = totalRecords == 0 ? 0 : (decimal)presentCount / totalRecords * 100;

        return new EmployeeAttendanceSummaryDto
        {
            EmployeeId = employee.Id,

            EmployeeName = $"{employee.FirstName} {employee.LastName}",

            TotalRecords = totalRecords,

            PresentCount = presentCount,

            AbsentCount = absentCount,

            LateCount = lateCount,

            LeaveCount = leaveCount,

            PresentPercentage = Math.Round(presentPercentage, 2),

            TotalWorkingHours = Math.Round(totalWorkingHours, 2),

            AverageWorkingHours = Math.Round(averageWorkingHours, 2)
        };
    }
}