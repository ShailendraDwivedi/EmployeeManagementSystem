using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Designations.Commands.CreateDesignation;
using EmployeeManagement.Application.Designations.Commands.DeleteDesignation;
using EmployeeManagement.Application.Designations.Commands.UpdateDesignation;
using EmployeeManagement.Application.Designations.DTOs;
using EmployeeManagement.Application.Designations.Queries.GetDesignationById;
using EmployeeManagement.Application.Designations.Queries.GetDesignations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public DesignationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDesignations([FromQuery] GetDesignationsQuery query, CancellationToken cancellationToken = default)
        {
            var designationsList = await _mediator.Send(query, cancellationToken);
            if (designationsList == null || designationsList.Items == null || !designationsList.Items.Any())
            {
                return NotFound(ApiResponse.Fail("Designation not found."));
            }

            return Ok(ApiResponse<PagedResult<DesignationDto>>.SuccessResponse(designationsList, "Designations retrieved successfully."));

        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDesignationById(Guid id, CancellationToken cancellationToken)
        {
            var designation = await _mediator.Send(new GetDesignationByIdQuery(id), cancellationToken);
            if (designation == null)
            {
                return NotFound(ApiResponse.Fail("Designation not found."));
            }
            return Ok(
                ApiResponse<DesignationDto>.SuccessResponse(designation, "Designation retrieved successfully."));
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDesignationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(ApiResponse<DesignationDto>.SuccessResponse(result, "Designation created successfully."));
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateDesignationCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            if (id != command.Id)
            {
                return NotFound(ApiResponse.Fail("Designation ID mismatch."));
            }

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(ApiResponse<DesignationDto>.SuccessResponse(result, "Designation updated successfully."));
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

            return Ok(ApiResponse.Ok("Designation deleted successfully."));
        }
    }
}
