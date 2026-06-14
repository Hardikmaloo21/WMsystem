// WMS.API/Controllers/v1/AttendanceController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Attendance.Commands;
using WMS.Application.Features.Attendance.DTOs;
using WMS.Application.Features.Attendance.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;
    public AttendanceController(IMediator mediator) => _mediator = mediator;

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckInCommand(dto), ct);
        return Ok(ApiResponse<AttendanceDto>.Success(result, "Checked in successfully."));
    }

    [HttpPost("check-out")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CheckOutCommand(dto), ct);
        return Ok(ApiResponse<AttendanceDto>.Success(result, "Checked out successfully."));
    }

    [HttpGet("employee/{empId:int}/monthly")]
    public async Task<IActionResult> GetMonthly(
        int empId, [FromQuery] int year, [FromQuery] int month, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetMonthlyAttendanceQuery(empId, year, month), ct);
        return Ok(ApiResponse<IReadOnlyList<AttendanceDto>>.Success(result));
    }

    [HttpGet("today")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> GetTodayAttendance(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTodayAttendanceQuery(), ct);
        return Ok(ApiResponse<object>.Success(result));
    }
}