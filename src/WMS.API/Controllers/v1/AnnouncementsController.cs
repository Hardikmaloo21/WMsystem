﻿

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Announcements.DTOs;
using WMS.Application.Features.Announcements.Commands;
// using WMS.Application.Features.Announcements.Queries;
[ApiController]

//[ApiVersion("1.0")]

[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class AnnouncementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AnnouncementsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetAnnouncementsQuery(isActive, pageNumber, pageSize), ct);
        return Ok(ApiResponse<object>.Success(new
        {
            result.Items,
            Pagination = new
            {
                result.PageNumber,
                result.PageSize,
                result.TotalCount,
                result.TotalPages
            }
        }));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(
        [FromBody] CreateAnnouncementDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateAnnouncementCommand(dto), ct);
        return Ok(ApiResponse<AnnouncementDto>.Success(
            result, "Announcement created.", 201));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(
        int id, [FromBody] UpdateAnnouncementDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new UpdateAnnouncementCommand(dto with { AnnouncementId = id }), ct);
        return Ok(ApiResponse<AnnouncementDto>.Success(result, "Announcement updated."));
    }

    [HttpPatch("{id:int}/toggle")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Toggle(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new ToggleAnnouncementCommand(id), ct);
        var msg = result.IsActive ? "Announcement activated." : "Announcement deactivated.";
        return Ok(ApiResponse<AnnouncementDto>.Success(result, msg));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAnnouncementCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Announcement deleted."));
    }

    [HttpGet("{id:int}")]
public async Task<IActionResult> GetById(
    int id,
    CancellationToken ct)
{
    var result = await _mediator.Send(
        new GetAnnouncementByIdQuery(id),
        ct);

    return Ok(
        ApiResponse<AnnouncementDto>.Success(result)
    );
}
}
