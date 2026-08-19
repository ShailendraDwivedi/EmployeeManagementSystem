using MediatR;

namespace EmployeeManagement.Application.Designations.Commands.DeleteDesignation;

public record DeleteDesignationCommand    : IRequest<bool>
{
      public Guid Id { get; set; }
}