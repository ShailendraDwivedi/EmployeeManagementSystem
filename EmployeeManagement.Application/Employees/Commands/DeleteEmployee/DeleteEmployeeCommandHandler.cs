using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Entities;
using MediatR;

namespace EmployeeManagement.Application.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee =
            await _unitOfWork.Employees.GetByIdAsync(request.Id);

        if (employee == null)
            throw new Exception("Employee not found.");
        // Soft delete
        employee.IsDeleted = true;
        employee.IsActive = false;
        employee.UpdatedDate = DateTime.UtcNow;
        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}