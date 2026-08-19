using EmployeeManagement.Application.Designations.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Designations.Queries.GetDesignationById;

public record GetDesignationByIdQuery(Guid Id) : IRequest<DesignationDto?>;