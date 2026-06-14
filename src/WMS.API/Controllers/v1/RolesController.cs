using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WMS.Application.Common.Models;
using WMS.Application.Features.Roles.DTOs;
using WMS.Application.Features.Roles.Queries;

namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetRolesQuery(),
            ct);

        return Ok(
            ApiResponse<IReadOnlyList<RoleDto>>
                .Success(result));
    }
}