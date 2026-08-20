using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Commands.CheckOut;

public class CheckOutCommandHandler
    : IRequestHandler<CheckOutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckOutCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        CheckOutCommand request,
        CancellationToken cancellationToken)
    {
        var attendance = await _unitOfWork.Attendances
            .Query().FirstOrDefaultAsync(
                x => x.AttendanceId == request.AttendanceId,
                cancellationToken);

        if (attendance == null)
        {
            throw new KeyNotFoundException(
                "Attendance record not found.");
        }

        if (attendance.CheckOut.HasValue)
        {
            throw new InvalidOperationException(
                "Employee has already checked out.");
        }

        var checkOut = DateTime.UtcNow;

        if (checkOut < attendance.CheckIn)
        {
            throw new InvalidOperationException(
                "Check-out time cannot be before check-in time.");
        }

        attendance.CheckOut = checkOut;

        var duration = checkOut - attendance.CheckIn;

        attendance.WorkingHours =
            Math.Round(
                (decimal)duration.TotalHours,
                2);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}