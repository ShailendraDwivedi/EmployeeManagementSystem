using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Departments.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Get department
        var department =
            await _unitOfWork.Departments.GetByIdAsync(request.Id);

        if (department == null)
        {
            throw new Exception("Department not found.");
        }

        // 2. Check duplicate name
        var duplicate =
            await _unitOfWork.Departments
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
        _mapper.Map(request, department);

        // 4. Update date
        department.UpdatedDate =
            DateTime.UtcNow;

        // 5. Update
        _unitOfWork.Departments
            .Update(department);

        // 6. Save
        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // 7. Return DTO
        return _mapper.Map<DepartmentDto>(
            department);
    }
}