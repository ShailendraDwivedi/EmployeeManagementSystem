using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Departments.Commands.DeleteDepartment;
using EmployeeManagement.Application.Departments.Commands.UpdateDepartment;
using EmployeeManagement.Application.Departments.DTOs;
using EmployeeManagement.Application.Departments.Queries.GetDepartmentById;
using EmployeeManagement.Application.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDepartments([FromQuery] GetDepartmentsQuery query, CancellationToken cancellationToken = default)
        {
            var departmentsList = await _mediator.Send(query, cancellationToken);
            if (departmentsList == null || departmentsList.Items == null || !departmentsList.Items.Any())
            {
                return NotFound(ApiResponse.Fail("Department not found."));
            }

            return Ok(ApiResponse<PagedResult<DepartmentDto>>.SuccessResponse(departmentsList, "Departments retrieved successfully."));

        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDepartmentById(Guid id, CancellationToken cancellationToken)
        {
            var departments = await _mediator.Send(new GetDepartmentByIdQuery(id), cancellationToken);
            if (departments == null)
            {
                return NotFound(ApiResponse.Fail("Department not found."));
            }
            return Ok(
                ApiResponse<DepartmentDto>.SuccessResponse(departments, "Department retrieved successfully."));
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(ApiResponse<DepartmentDto>.SuccessResponse(result, "Department created successfully."));
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            if (id != command.Id)
            {
                return NotFound(ApiResponse.Fail("Department ID mismatch."));
            }

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(ApiResponse<DepartmentDto>.SuccessResponse(result, "Department updated successfully."));
        }
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDesignationCommand
            {
                Id = id
            };
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.Ok("Department deleted successfully."));
        }
    }
}
