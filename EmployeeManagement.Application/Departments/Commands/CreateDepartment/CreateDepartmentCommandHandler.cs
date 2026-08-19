using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Departments.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentName = request.Name.Trim();
        var departmentExists = await _unitOfWork.Departments
                .Query()
                .AnyAsync(
                    x => x.Name == departmentName &&
                         !x.IsDeleted,
                    cancellationToken);

        if (departmentExists)
        {
            throw new ValidationException(
                "A department with this name already exists.");
        }

        var department = _mapper.Map<Department>(request);

        department.Id = Guid.NewGuid();
        department.CreatedDate = DateTime.UtcNow;
        department.UpdatedDate = DateTime.UtcNow;
        department.IsDeleted = false;

        await _unitOfWork.Departments.AddAsync(department, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DepartmentDto>(department);
    }
}