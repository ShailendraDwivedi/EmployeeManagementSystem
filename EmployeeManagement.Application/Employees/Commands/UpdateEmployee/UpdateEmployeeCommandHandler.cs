using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee =
            await _unitOfWork.Employees.GetByIdAsync(request.Id);

        if (employee == null)
            throw new Exception("Employee not found.");

        var department =
            await _unitOfWork.Departments
                .GetByIdAsync(
                    request.DepartmentId);

        if (department == null ||
            department.IsDeleted)
        {
            throw new Exception(
                "Invalid DepartmentId.");
        }
        var designation =
            await _unitOfWork.Designations
                .GetByIdAsync(
                    request.DesignationId);

        if (designation == null ||
            designation.IsDeleted)
        {
            throw new Exception(
                "Invalid DesignationId.");
        }

        _mapper.Map(request, employee);
        employee.UpdatedDate = DateTime.UtcNow;

        _unitOfWork.Employees.Update(employee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // 7. Load Department + Designation for DTO
        var updatedEmployee = await _unitOfWork.Employees
            .Query()
            .Include(x => x.Department)
            .Include(x => x.Designation)
            .AsNoTracking()
            .FirstAsync(
                x => x.Id == employee.Id,
                cancellationToken);

        return _mapper.Map<EmployeeDto>(updatedEmployee);

    }
}