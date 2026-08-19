using MediatR;

namespace EmployeeManagement.Application.Departments.Commands.DeleteDepartment;

public record DeleteDesignationCommand    : IRequest<bool>
{
      public Guid Id { get; set; }
}