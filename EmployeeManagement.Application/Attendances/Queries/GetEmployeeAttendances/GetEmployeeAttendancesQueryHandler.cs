using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendances;

public class GetEmployeeAttendancesQueryHandler : IRequestHandler<GetEmployeeAttendancesQuery, PagedResult<AttendanceListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeeAttendancesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<AttendanceListDto>> Handle(GetEmployeeAttendancesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Attendances.Query().AsNoTracking()
                .Include(x => x.Employee)
                .Where(x =>
                    x.EmployeeId == request.EmployeeId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
                .OrderByDescending(x => x.CheckIn)
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<AttendanceListDto>(
                    _mapper.ConfigurationProvider)
                .ToListAsync(
                    cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PagedResult<AttendanceListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}