using EmployeeManagement.Application.Designations.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Designations.Commands.UpdateDesignation;

public class UpdateDesignationCommand : IRequest<DesignationDto>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}