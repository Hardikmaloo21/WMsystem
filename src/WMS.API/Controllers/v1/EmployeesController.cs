// WMS.API/Controllers/v1/EmployeesController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Employees.Commands;
using WMS.Application.Features.Employees.DTOs;
using WMS.Application.Features.Employees.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    public EmployeesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? departmentId,
        [FromQuery] int? roleId,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetEmployeesQuery(search, departmentId, roleId, status, pageNumber, pageSize), ct);

        return Ok(ApiResponse<object>.Success(new
        {
            result.Items,
            Pagination = new
            {
                result.PageNumber, result.PageSize,
                result.TotalCount, result.TotalPages
            }
        }));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), ct);
        return Ok(ApiResponse<EmployeeDto>.Success(result));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateEmployeeCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.EmployeeId },
            ApiResponse<EmployeeDto>.Success(result, "Employee created successfully.", 201));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto, CancellationToken ct)
    {
        if (id != dto.EmployeeId)
            return BadRequest(ApiResponse<object>.Fail("ID mismatch."));

        var result = await _mediator.Send(new UpdateEmployeeCommand(dto), ct);
        return Ok(ApiResponse<EmployeeDto>.Success(result, "Employee updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Employee deactivated successfully."));
    }

    [HttpPut("{id:int}/status")]
[Authorize(Policy = "AdminOnly")]
public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] UpdateEmployeeStatusDto dto,
    CancellationToken ct)
{
    await _mediator.Send(
        new UpdateEmployeeStatusCommand(
            id,
            dto.Status
        ),
        ct);

    return Ok(
        ApiResponse<object>.Success(
            null,
            "Employee status updated successfully."
        ));
}
}