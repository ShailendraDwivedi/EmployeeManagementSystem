using AutoMapper;
using EmployeeManagement.Application.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Departments.Commands.UpdateDepartment;
using EmployeeManagement.Application.Departments.DTOs;
using EmployeeManagement.Application.Designations.Commands.CreateDesignation;
using EmployeeManagement.Application.Designations.Commands.UpdateDesignation;
using EmployeeManagement.Application.Designations.DTOs;
using EmployeeManagement.Application.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Employees.Commands.UpdateEmployee;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee mappings
        CreateMap<Employee, EmployeeDto>()
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom(
                    src => src.Department != null
                        ? src.Department.Name
                        : string.Empty))
            .ForMember(
                dest => dest.DesignationName,
                opt => opt.MapFrom(
                    src => src.Designation != null
                        ? src.Designation.Name
                        : string.Empty));

        CreateMap<CreateEmployeeCommand, Employee>();
        CreateMap<UpdateEmployeeCommand, Employee>();

        // Department mappings

        CreateMap<CreateDepartmentCommand, Department>();

        CreateMap<Department, DepartmentDto>();

        CreateMap<UpdateDepartmentCommand, Department>();

        // Designation mappings

        CreateMap<CreateDesignationCommand, Designation>();

        CreateMap<Designation, DesignationDto>();
        CreateMap<UpdateDesignationCommand, Designation>();
    }
}