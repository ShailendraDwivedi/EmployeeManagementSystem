using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Departments.Commands.DeleteDepartment;

public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDesignationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        // 1. Find department
        var department = await _unitOfWork.Departments.GetByIdAsync(request.Id);

        if (department == null || department.IsDeleted)
        {
            throw new Exception(
                "Department not found.");
        }
        // 2. Check employees
        var hasEmployees =
            await _unitOfWork.Employees
                .Query()
                .AnyAsync(
                    x => x.DepartmentId == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

        if (hasEmployees)
        {
            throw new Exception(
                "Department cannot be deleted because " +
                "employees are assigned to it.");
        }
        // 2. Soft delete
        department.IsDeleted = true;

        department.UpdatedDate = DateTime.UtcNow;

        // 3. Update entity
        _unitOfWork.Departments.Update(department);

        // 4. Save
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}