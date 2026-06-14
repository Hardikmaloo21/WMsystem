// WMS.API/Controllers/v1/ClientsController.cs
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using WMS.Application.Features.Clients.Commands;
using WMS.Application.Features.Clients.DTOs;
using WMS.Application.Features.Clients.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetClientsQuery(search, pageNumber, pageSize), ct);
        return Ok(ApiResponse<object>.Success(new { result.Items, result.TotalCount }));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(id), ct);
        return Ok(ApiResponse<ClientDto>.Success(result));
    }

    [HttpPost]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateClientCommand(dto), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.ClientId },
            ApiResponse<ClientDto>.Success(result, "Client created.", 201));
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "ManagerAndAbove")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto dto, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateClientCommand(dto with { ClientId = id }), ct);
        return Ok(ApiResponse<ClientDto>.Success(result, "Client updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteClientCommand(id), ct);
        return Ok(ApiResponse<object>.Success(null, "Client deleted."));
    }
}