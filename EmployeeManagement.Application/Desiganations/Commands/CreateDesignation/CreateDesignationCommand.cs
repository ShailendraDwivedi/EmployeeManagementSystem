using EmployeeManagement.Application.Designations.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Designations.Commands.CreateDesignation;

public class CreateDesignationCommand : IRequest<DesignationDto>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDeleted { get; set; } = true;
}