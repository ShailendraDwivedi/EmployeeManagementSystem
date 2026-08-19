using EmployeeManagement.Application.Authentication.Commands.Login;
using EmployeeManagement.Application.Authentication.Commands.RefreshToken;
using EmployeeManagement.Application.Authentication.Commands.Register;
using EmployeeManagement.Application.Authentication.DTOs;
using EmployeeManagement.Application.Authentication.Responses;
using EmployeeManagement.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // =========================
    // REGISTER
    // =========================

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var message =
            await _mediator.Send(
                command,
                cancellationToken);

        return Ok(new
        {
            success = true,
            message = message
        });
    }

    // =========================
    // LOGIN
    // =========================

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDTO request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var response =
            await _mediator.Send(
                command,
                cancellationToken);

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            data = response
        });
    }

    // =========================
    // REFRESH TOKEN
    // =========================

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result =
            await _mediator.Send(
                command,
                cancellationToken);

        return Ok(
            ApiResponse<AuthResponse>
                .SuccessResponse(
                    result,
                    "Token refreshed successfully."));
    }
}