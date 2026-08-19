using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Designations.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Designations.Commands.UpdateDesignation;

public class UpdateDesignationCommandHandler : IRequestHandler<UpdateDesignationCommand, DesignationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDesignationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DesignationDto> Handle(UpdateDesignationCommand request, CancellationToken cancellationToken)
    {
        // 1. Get designation
        var designation =
            await _unitOfWork.Designations.GetByIdAsync(request.Id);

        if (designation == null)
        {
            throw new Exception("Designation not found.");
        }

        // 2. Check duplicate name
        var duplicate =
            await _unitOfWork.Designations
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Name == request.Name.Trim() && x.IsActive &&
                         !x.IsDeleted,
                    cancellationToken);

        if (duplicate)
        {
            throw new Exception(
                "A department with this name already exists");
        }

        // 3. Map request to entity
        _mapper.Map(request, designation);

        // 4. Update date
        designation.UpdatedDate =
            DateTime.UtcNow;

        // 5. Update
        _unitOfWork.Designations
            .Update(designation);

        // 6. Save
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 7. Return DTO
        return _mapper.Map<DesignationDto>(
            designation);
    }
}