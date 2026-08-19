using EmployeeManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Designations.Commands.DeleteDesignation;

public class DeleteDesignationCommandHandler : IRequestHandler<DeleteDesignationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDesignationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {
        // 1. Find designation
        var designation = await _unitOfWork.Designations.GetByIdAsync(request.Id);  
        if (designation == null || designation.IsDeleted)
        {
            throw new Exception(
                "Designation not found.");
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
                "Designation cannot be deleted because " +
                "employees are assigned to it.");
        }
        // 2. Soft delete
        designation.IsDeleted = true;
        designation.UpdatedDate = DateTime.UtcNow;

        // 3. Update entity
        _unitOfWork.Designations.Update(designation);

        // 4. Save
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}