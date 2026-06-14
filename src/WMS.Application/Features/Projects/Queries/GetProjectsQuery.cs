

// ─────────────────────────────────────────────
// Queries
// WMS.Application/Features/Projects/Queries/
// ─────────────────────────────────────────────

using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Projects.DTOs;
using WMS.Domain.Common;


public record GetProjectsQuery(

    string? Search,
    string? Status,
    int? ClientId,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<ProjectDto>>;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, PagedResult<ProjectDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProjectDto>> Handle(
        GetProjectsQuery request, CancellationToken ct)
    {
        var query = _uow.Projects.Query()
            .Include(p => p.Client)
            .Include(p => p.Allocations)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(p => p.ProjectName.Contains(request.Search));

        if (!string.IsNullOrWhiteSpace(request.Status))
            query = query.Where(p => p.Status == request.Status);

        if (request.ClientId.HasValue)
            query = query.Where(p => p.ClientId == request.ClientId.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<ProjectDto>
        {
            Items = _mapper.Map<IReadOnlyList<ProjectDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
