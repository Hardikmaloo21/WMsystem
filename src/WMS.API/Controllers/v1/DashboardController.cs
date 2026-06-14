﻿

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
// using WMS.Application.Features.Dashboard.DTOs;
using WMS.Application.Features.Dashboard.Queries;



[ApiController]

//[ApiVersion("1.0")]

[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    public DashboardController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Returns full admin dashboard — KPI cards + all chart datasets.
    /// </summary>
    // [HttpGet("summary")]
    // [Authorize(Policy = "ManagerAndAbove")]
    // public async Task<IActionResult> GetSummary(CancellationToken ct)
    // {
    //     var result = await _mediator.Send(new GetDashboardSummaryQuery(), ct);
    //     return Ok(ApiResponse<DashboardSummaryDto>.Success(result));
    // }

    [HttpGet("summary")]
[Authorize(Policy = "AllRoles")]
public async Task<IActionResult> GetSummary(
    CancellationToken ct)
{
    var result =
        await _mediator.Send(
            new GetDashboardSummaryQuery(),
            ct);

    return Ok(
        ApiResponse<DashboardSummaryDto>
            .Success(result)
    );
}
}