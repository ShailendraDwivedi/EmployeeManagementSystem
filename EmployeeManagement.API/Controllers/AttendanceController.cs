using EmployeeManagement.Application.Attendances.Commands.CheckIn;
using EmployeeManagement.Application.Attendances.Commands.CheckOut;
using EmployeeManagement.Application.Attendances.DTOs;
using EmployeeManagement.Application.Attendances.Queries.GetAttendanceById;
using EmployeeManagement.Application.Attendances.Queries.GetAttendanceDashboard;
using EmployeeManagement.Application.Attendances.Queries.GetAttendances;
using EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendances;
using EmployeeManagement.Application.Attendances.Queries.GetEmployeeAttendanceSummary;
using EmployeeManagement.Application.Common.Models;
using EmployeeManagement.Application.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/attendance/check-in
    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInCommand command)
    {
        var attendanceId = await _mediator.Send(command);

        return Ok(new
        {
            AttendanceId = attendanceId,
            Message = "Employee checked in successfully."
        });
    }

    // PUT: api/attendance/check-out/{attendanceId}
    [HttpPut("check-out/{attendanceId:guid}")]
    public async Task<IActionResult> CheckOut(Guid attendanceId)
    {
        var command = new CheckOutCommand
        {
            AttendanceId = attendanceId
        };

        await _mediator.Send(command);

        return Ok(new
        {
            Message = "Employee checked out successfully."
        });
    }

    // GET: api/attendance
    [HttpGet]
    public async Task<IActionResult> GetAttendances(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] Guid? employeeId = null,
    [FromQuery] DateTime? fromDate = null,
    [FromQuery] DateTime? toDate = null,
    [FromQuery] string? status = null)
    {
        var query = new GetAttendancesQuery(
            pageNumber,
            pageSize,
            search,
            employeeId,
            fromDate,
            toDate,
            status);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    //public async Task<IActionResult> GetAttendances([FromQuery] GetAttendancesQuery query, CancellationToken cancellationToken = default)
    //{
    //    var result = await _mediator.Send(query, cancellationToken);

    //    if (result == null || result.Items == null || !result.Items.Any())
    //    {
    //        return NotFound(ApiResponse.Fail("Department not found."));
    //    }
    //    return Ok(ApiResponse<PagedResult<AttendanceListDto>>.SuccessResponse(result, "Attendence retrieved successfully."));
    //}

    // GET: api/attendance/{attendanceId}
    [HttpGet("{attendanceId:guid}")]
    public async Task<IActionResult> GetById(Guid attendanceId)
    {
        var result = await _mediator.Send(new GetAttendanceByIdQuery(attendanceId));

        if (result == null)
        {
            return NotFound(new
            {
                Message = "Attendance record not found."
            });
        }

        return Ok(result);
    }
    [Authorize]
    // GET: api/attendance/employee/{employeeId}
    [HttpGet("employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeAttendance(Guid employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetEmployeeAttendancesQuery
        {
            EmployeeId = employeeId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<AttendanceDashboardDto>> GetDashboard([FromQuery] int year, [FromQuery] int month)
    {
        var result = await _mediator.Send(new GetAttendanceDashboardQuery(year, month));

        return Ok(result);
    }
    [HttpGet("employee-summary")]
    public async Task<ActionResult<EmployeeAttendanceSummaryDto?>> GetEmployeeSummary([FromQuery] Guid employeeId, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _mediator.Send(
            new GetEmployeeAttendanceSummaryQuery(
                employeeId,
                year,
                month));

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}