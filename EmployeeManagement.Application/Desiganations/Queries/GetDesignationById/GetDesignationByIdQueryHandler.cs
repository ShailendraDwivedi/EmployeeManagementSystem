using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Designations.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Designations.Queries.GetDesignationById;
public class GetDesignationByIdQueryHandler : IRequestHandler<GetDesignationByIdQuery, DesignationDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDesignationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DesignationDto?> Handle(GetDesignationByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _unitOfWork.Designations
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

        if (department == null)
        {
            return null;
        }

        return _mapper.Map<DesignationDto>(department);
    }
}