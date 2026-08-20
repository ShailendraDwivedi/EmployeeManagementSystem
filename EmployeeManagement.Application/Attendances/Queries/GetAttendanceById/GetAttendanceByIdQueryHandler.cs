using AutoMapper;
using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendanceById;

public class GetAttendanceByIdQueryHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAttendanceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AttendanceDto?> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
    {
        var attendance = await _unitOfWork.Attendances.Query().AsNoTracking().Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.AttendanceId == request.AttendanceId, cancellationToken);

        if (attendance == null)
        {
            return null;
        }

        return _mapper.Map<AttendanceDto>(attendance);
    }
}