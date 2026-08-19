using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Designations.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagement.Application.Designations.Queries.GetDesignations
{
    public class GetDesignationsQueryHandler : IRequestHandler<GetDesignationsQuery, PagedResult<DesignationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDesignationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PagedResult<DesignationDto>> Handle(GetDesignationsQuery request, CancellationToken cancellationToken)
        {
            // 1. Normalize pagination
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            // 2. Build IQueryable
            var query = _unitOfWork.Designations.Query().AsNoTracking().Where(x => !x.IsDeleted);
            // 3. Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(x => x.Name.Contains(search));
            }

            // 4. Total count BEFORE pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // 5. Sorting
            query = request.SortBy?.ToLower() switch
            {
                "name" =>
                    request.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(x => x.Name)
                        : query.OrderBy(x => x.Name),

                _ =>
                    query.OrderBy(x => x.Name)
            };

            // 6. Pagination
            var designations = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

            // 7. Map to DTO
            var designationDtos = _mapper.Map<List<DesignationDto>>(
                    designations);

            // 8. Calculate total pages
            var totalPages = (int)Math.Ceiling(
                    totalCount / (double)pageSize);

            // 9. Return result
            return new PagedResult<DesignationDto>
            {
                Items = designationDtos,

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount,

                TotalPages = totalPages
            };
        }

    }
}
