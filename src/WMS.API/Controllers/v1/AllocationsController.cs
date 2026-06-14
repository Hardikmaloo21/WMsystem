// File: WMS.API/Controllers/v1/AllocationsController.cs
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using WMS.Application.Common.Models;
using WMS.Application.Features.Allocations.Commands;
using WMS.Application.Features.Allocations.DTOs;
using WMS.Application.Features.Allocations.Queries;


namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class AllocationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AllocationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? empId,
        [FromQuery] int? projectId,
        [FromQuery] bool? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetAllocationsQuery(empId, projectId, status, pageNumber, pageSize), ct);
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

    [HttpPost("assign")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Assign(
        [FromBody] AssignEmployeeDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new AssignEmployeeCommand(dto), ct);
        return Ok(ApiResponse<AllocationDto>.Success(
            result, "Employee assigned to project successfully."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Update(
        int id, [FromBody] UpdateAllocationDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new UpdateAllocationCommand(dto with { AllocationId = id }), ct);
        return Ok(ApiResponse<AllocationDto>.Success(result, "Allocation updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
    {
        await _mediator.Send(new RemoveAllocationCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Allocation removed."));
    }

    [HttpGet("employee/{empId:int}/history")]
    public async Task<IActionResult> GetHistory(int empId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetAllocationsQuery(EmpId: empId, ProjectId: null, Status: null, PageNumber: 1, PageSize: 100), ct);
        return Ok(ApiResponse<object>.Success(result.Items));
    }

    [HttpGet("{id:int}")]
public async Task<IActionResult> GetById(
    int id,
    CancellationToken ct)
{
    var result = await _mediator.Send(
        new GetAllocationByIdQuery(id),
        ct);

    return Ok(
        ApiResponse<AllocationDto>.Success(result)
    );
}

}
