﻿

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Projects.DTOs;
using WMS.Application.Features.Projects.Commands;

[ApiController]

//[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] int? clientId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetProjectsQuery(search, status, clientId, pageNumber, pageSize), ct);
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id), ct);
        return Ok(ApiResponse<ProjectDto>.Success(result));
    }

    [HttpPost]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Create(
        [FromBody] CreateProjectDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateProjectCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ProjectId },
            ApiResponse<ProjectDto>.Success(result, "Project created successfully.", 201));
    }

    [HttpPut("{id:int}")]
[Authorize(Policy = "ManagerAndAbove")]
public async Task<IActionResult> Update(
    int id, [FromBody] UpdateProjectDto dto, CancellationToken ct)
{
    dto.ProjectId = id;

    var result = await _mediator.Send(
        new UpdateProjectCommand(dto), ct);

    return Ok(ApiResponse<ProjectDto>.Success(
        result,
        "Project updated successfully."));
}

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteProjectCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Project deleted successfully."));
    }
}
