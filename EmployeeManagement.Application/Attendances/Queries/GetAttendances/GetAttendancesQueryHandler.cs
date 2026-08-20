using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Attendances.Queries.GetAttendances;

public class GetAttendancesQueryHandler : IRequestHandler<GetAttendancesQuery, PagedResult<AttendanceListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAttendancesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<AttendanceListDto>> Handle(GetAttendancesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Attendances
            .Query()
            .AsNoTracking()
            .Include(x => x.Employee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Employee.FirstName.Contains(search) ||
                x.Employee.LastName.Contains(search) ||
                x.Employee.Email.Contains(search));
        }

        if (request.EmployeeId.HasValue)
        {
            query = query.Where(x =>
                x.EmployeeId == request.EmployeeId.Value);
        }

        if (request.FromDate.HasValue)
        {
            var fromDate = request.FromDate.Value.Date;

            query = query.Where(x =>
                x.CheckIn >= fromDate);
        }

        if (request.ToDate.HasValue)
        {
            var toDate =
                request.ToDate.Value.Date.AddDays(1);

            query = query.Where(x =>
                x.CheckIn < toDate);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status.Equals(
                "CheckedIn",
                StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x =>
                    x.CheckOut == null);
            }
            else if (request.Status.Equals(
                "CheckedOut",
                StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x =>
                    x.CheckOut != null);
            }
        }

        var totalCount =
            await query.CountAsync(cancellationToken);

        var items =
            await query
                .OrderByDescending(x => x.CheckIn)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<AttendanceListDto>(
                    _mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        var totalPages =
            (int)Math.Ceiling(
                totalCount / (double)request.PageSize);

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