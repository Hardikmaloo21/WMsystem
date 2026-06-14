// WMS.API/Controllers/v1/LeavesController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Leaves.Commands;
using WMS.Application.Features.Leaves.DTOs;
using WMS.Application.Features.Leaves.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class LeavesController : ControllerBase
{
    private readonly IMediator _mediator;
    public LeavesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new ApplyLeaveCommand(dto), ct);
        return Ok(ApiResponse<LeaveDto>.Success(result, "Leave applied successfully."));
    }

    [HttpPost("approve-reject")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> ApproveReject([FromBody] ApproveRejectLeaveDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new ApproveLeaveCommand(dto), ct);
        return Ok(ApiResponse<LeaveDto>.Success(result, $"Leave {dto.Action}d successfully."));
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        await _mediator.Send(new CancelLeaveCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Leave cancelled."));
    }

    [HttpGet("employee/{empId:int}")]
    public async Task<IActionResult> GetByEmployee(
        int empId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetLeavesByEmployeeQuery(empId, pageNumber, pageSize), ct);
        return Ok(ApiResponse<object>.Success(new { result.Items, result.TotalCount }));
    }

    [HttpGet("pending")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> GetPending(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetPendingLeavesQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<LeaveDto>>.Success(result));
    }
}