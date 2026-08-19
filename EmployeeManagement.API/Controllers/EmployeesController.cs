using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Employees.Commands.DeleteEmployee;
using EmployeeManagement.Application.Employees.Commands.UpdateEmployee;
using EmployeeManagement.Application.Employees.DTOs;
using EmployeeManagement.Application.Employees.Queries.GetEmployeeById;
using EmployeeManagement.Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "EMPLOYEE,HR,ADMIN")]
        public async Task<IActionResult> GetEmployees([FromQuery] GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employeeList = await _mediator.Send(query, cancellationToken);
            if (employeeList == null || employeeList.Items == null || !employeeList.Items.Any())
            {
                return NotFound(ApiResponse.Fail("Employee not found."));
            }

            return Ok(ApiResponse<PagedResult<EmployeeDto>>.SuccessResponse(employeeList, "Employees retrieved successfully."));
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "EMPLOYEE, HR, ADMIN")]
        public async Task<IActionResult> GetEmployee(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));

            if (employee == null)
            {
                return NotFound(ApiResponse.Fail("Employee not found."));
            }

            return Ok(
                ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee retrieved successfully."));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,HR,EMPLOYEE")]
        public async Task<IActionResult> Create(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee created successfully."));
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "ADMIN,HR, EMPLOYEE")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var employee = await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse<EmployeeDto>.SuccessResponse(employee, "Employee updated successfully."));
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteEmployeeCommand
            {
                Id = id
            };
            await _mediator.Send(command, cancellationToken);

            return Ok(ApiResponse.Ok("Employee deleted successfully."));
        }
        [HttpGet("claims")]
        [Authorize]
        public IActionResult GetClaims()
        {
            var claims = User.Claims
                .Select(c => new
                {
                    c.Type,
                    c.Value
                });

            return Ok(claims);
        }
    }
}
