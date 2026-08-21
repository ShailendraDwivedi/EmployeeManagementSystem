using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Commands.CheckIn;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckInCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        // Check employee exists
        var employee = await _unitOfWork.Employees
            .Query().FirstOrDefaultAsync(x => x.Id == request.EmployeeId &&
                     !x.IsDeleted,
                cancellationToken);

        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found.");
        }

        // Check whether employee already checked in
        var existingAttendance = await _unitOfWork.Attendances
            .Query().FirstOrDefaultAsync(
                x => x.EmployeeId == request.EmployeeId &&
                     x.CheckOut == null &&
                     x.CheckIn.Date == DateTime.UtcNow.Date,
                cancellationToken);

        if (existingAttendance != null)
        {
            throw new InvalidOperationException("Employee has already checked in.");
        }

        var attendance = new Attendance
        {
            AttendanceId = Guid.NewGuid(),
            EmployeeId = request.EmployeeId,
            CheckIn = DateTime.UtcNow,
            CheckOut = null,
            WorkingHours = null,
            Status = AttendanceStatus.Present,
            AttendanceDate = DateTime.UtcNow.Date
        };

        await _unitOfWork.Attendances.AddAsync(attendance, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return attendance.AttendanceId;
    }
}