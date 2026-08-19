using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;


namespace EmployeeManagement.Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmployeeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var department = await _unitOfWork.Departments
                .GetByIdAsync(request.DepartmentId);

        if (department == null || department.IsDeleted)
        {
            throw new KeyNotFoundException($"Department with Id '{request.DepartmentId}' does not exist.");
        }
        var designation = await _unitOfWork.Designations
                .GetByIdAsync(request.DesignationId);

        if (designation == null || designation.IsDeleted)
        {
            throw new KeyNotFoundException($"Designation with Id '{request.DesignationId}' does not exist.");
        }
        var employee = _mapper.Map<Employee>(request);
        employee.Id = Guid.NewGuid();
        employee.IsActive = true;
        employee.IsDeleted = false;
        employee.CreatedDate = DateTime.UtcNow;
        employee.UpdatedDate = DateTime.UtcNow;

        await _unitOfWork.Employees.AddAsync(employee, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EmployeeDto>(employee);
    }
}