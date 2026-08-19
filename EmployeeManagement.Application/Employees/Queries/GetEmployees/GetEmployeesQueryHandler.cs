using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace EmployeeManagement.Application.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PagedResult<EmployeeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1
            ? 1
            : request.PageNumber;

        var pageSize = request.PageSize < 1
            ? 10
            : request.PageSize;

        if (pageSize > 100)
        {
            pageSize = 100;
        }
        // Keep this as IQueryable
        var query = _unitOfWork.Employees.Query().Include(x => x.Department).Include(x => x.Designation).AsNoTracking()
            .Where(x => !x.IsDeleted);

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search) ||
                x.Email.Contains(search) || x.Department!.Name.Contains(search) || x.Designation!.Name.Contains(search) ||
                x.EmployeeCode.Contains(search));
        }
        if (request.DepartmentId.HasValue)
        {
            query = query.Where(x =>
                x.DepartmentId ==
                request.DepartmentId.Value);
        }
        if (request.DesignationId.HasValue)
        {
            query = query.Where(x =>
                x.DesignationId ==
                request.DesignationId.Value);
        }

        var totalCount =
           await query.CountAsync(cancellationToken);
        query = request.SortBy?.ToLower() switch
        {
            "firstname" =>
                request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.FirstName)
                    : query.OrderBy(x => x.FirstName),

            "lastname" =>
                request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.LastName)
                    : query.OrderBy(x => x.LastName),

            "email" =>
                request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.Email)
                    : query.OrderBy(x => x.Email),

            "salary" =>
                request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.Salary)
                    : query.OrderBy(x => x.Salary),

            "joiningdate" =>
                request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.JoiningDate)
                    : query.OrderBy(x => x.JoiningDate),

            _ =>
                query.OrderBy(x => x.FirstName)
        };


        // Order + Projection + Execute query
        var employees = await query
           .Skip((pageNumber - 1) * request.PageSize)
           .Take(request.PageSize).ToListAsync(cancellationToken);

        var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResult<EmployeeDto>
        {
            Items = employeeDtos,

            PageNumber = request.PageNumber,

            PageSize = request.PageSize,

            TotalCount = totalCount,

            TotalPages = totalPages
        };
    }
}