using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Roles.DTOs;

namespace WMS.Application.Features.Roles.Queries;

public record GetRolesQuery : IRequest<IReadOnlyList<RoleDto>>;

public class GetRolesQueryHandler
    : IRequestHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    private readonly IUnitOfWork _uow;

    public GetRolesQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IReadOnlyList<RoleDto>> Handle(
        GetRolesQuery request,
        CancellationToken ct)
    {
        return await _uow.Roles.Query()
            .Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Description = r.Description
            })
            .ToListAsync(ct);
    }
}