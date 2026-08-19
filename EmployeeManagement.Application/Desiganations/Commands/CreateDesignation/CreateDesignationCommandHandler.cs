using AutoMapper;
using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Application.Designations.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.Designations.Commands.CreateDesignation;

public class CreateDesignationCommandHandler : IRequestHandler<CreateDesignationCommand, DesignationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDesignationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DesignationDto> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
    {
        var designationName = request.Name.Trim();

        var designationExists = await _unitOfWork.Designations
                .Query()
                .AnyAsync(
                    x => x.Name == designationName &&
                         !x.IsDeleted,
                    cancellationToken);

        if (designationExists)
        {
            throw new ValidationException(
                "A designation with this name already exists.");
        }

        var designation = _mapper.Map<Designation>(request);

        designation.Id = Guid.NewGuid();
        designation.CreatedDate = DateTime.UtcNow;
        designation.UpdatedDate = DateTime.UtcNow;
        designation.IsDeleted = false;

        await _unitOfWork.Designations.AddAsync(designation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DesignationDto>(designation);
    }
}