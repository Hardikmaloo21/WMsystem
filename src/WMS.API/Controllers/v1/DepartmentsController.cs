// WMS.API/Controllers/v1/DepartmentsController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Departments.Commands;
using WMS.Application.Features.Departments.DTOs;
using WMS.Application.Features.Departments.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepartmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDepartmentsQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<DepartmentDto>>.Success(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id), ct);
        return Ok(ApiResponse<DepartmentDto>.Success(result));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateDepartmentCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.DepartmentId },
            ApiResponse<DepartmentDto>.Success(result, "Department created.", 201));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateDepartmentCommand(dto with { DepartmentId = id }), ct);
        return Ok(ApiResponse<DepartmentDto>.Success(result, "Department updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteDepartmentCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Department deleted."));
    }
}