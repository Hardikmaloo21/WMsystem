// WMS.API/Controllers/v1/AuthController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.Common.Models;
using WMS.Application.Features.Auth.Commands;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>Authenticate user and return JWT tokens.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Login successful."));
    }

    /// <summary>Refresh expired access token.</summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(ApiResponse<LoginResponse>.Success(result, "Token refreshed."));
    }

    /// <summary>Invalidate refresh token and log out.</summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var username = User.Identity?.Name ?? string.Empty;
        await _mediator.Send(new LogoutCommand(username), ct);
        return Ok(ApiResponse<object>.Success(null, "Logged out successfully."));
    }

    /// <summary>Change password for authenticated user.</summary>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken ct)
    {
        var username = User.Identity?.Name ?? string.Empty;
        var cmd = command with { Username = username };
        await _mediator.Send(cmd, ct);
        return Ok(ApiResponse<object>.Success(null, "Password changed successfully."));
    }
}