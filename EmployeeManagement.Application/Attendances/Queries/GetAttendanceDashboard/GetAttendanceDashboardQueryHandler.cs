using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendanceDashboard;

public class GetAttendanceDashboardQueryHandler : IRequestHandler<GetAttendanceDashboardQuery, AttendanceDashboardDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAttendanceDashboardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AttendanceDashboardDto> Handle(GetAttendanceDashboardQuery request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(request.Year, request.Month, 1);

        var endDate = startDate.AddMonths(1);

        var records = await _unitOfWork.Attendances
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.AttendanceDate >= startDate &&
                x.AttendanceDate < endDate)
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

        decimal presentPercentage = totalRecords == 0 ? 0 : Math.Round((decimal)presentCount / totalRecords * 100, 2);

        // Calculate working hours
        var workingRecords = records
            .Where(x =>
                x.CheckIn != default &&
                x.CheckOut != default &&
                x.CheckOut > x.CheckIn)
            .ToList();

        decimal totalWorkingHours = workingRecords.Sum(x => (decimal)(x.CheckOut! - x.CheckIn!).Value.TotalHours);

        decimal averageWorkingHours = workingRecords.Count == 0 ? 0 : totalWorkingHours / workingRecords.Count;

        var monthlySummary = new AttendanceMonthlySummaryDto
        {
            Year = request.Year,
            Month = request.Month,

            TotalDays = totalRecords,

            PresentDays = presentCount,

            AbsentDays = absentCount,

            LateDays = lateCount,

            LeaveDays = leaveCount,

            PresentPercentage = presentPercentage,

            TotalWorkingHours = Math.Round(totalWorkingHours, 2),

            AverageWorkingHours = Math.Round(averageWorkingHours, 2)
        };

        return new AttendanceDashboardDto
        {
            TotalRecords = totalRecords,

            PresentCount = presentCount,

            AbsentCount = absentCount,

            LateCount = lateCount,

            LeaveCount = leaveCount,

            PresentPercentage = presentPercentage,

            TotalWorkingHours = Math.Round(totalWorkingHours, 2),

            AverageWorkingHours = Math.Round(averageWorkingHours, 2),

            MonthlySummary = new List<AttendanceMonthlySummaryDto>
        {
            monthlySummary
        }
        };
    }
}