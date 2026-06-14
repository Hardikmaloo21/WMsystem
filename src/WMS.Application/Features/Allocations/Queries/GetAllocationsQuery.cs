// File: WMS.Application/Features/Allocations/Queries/GetAllocationsQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Allocations.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Allocations.Queries;


public record GetAllocationsQuery(
    int? EmpId,
    int? ProjectId,
    bool? Status,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<AllocationDto>>;

public class GetAllocationsQueryHandler
    : IRequestHandler<GetAllocationsQuery, PagedResult<AllocationDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllocationsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<AllocationDto>> Handle(
        GetAllocationsQuery request, CancellationToken ct)
    {
        var query = _uow.Allocations.Query()
            .Include(a => a.Employee).ThenInclude(e => e.Department)
            .Include(a => a.Project)
            .AsNoTracking();

        if (request.EmpId.HasValue)
            query = query.Where(a => a.EmpId == request.EmpId.Value);

        if (request.ProjectId.HasValue)
            query = query.Where(a => a.ProjectId == request.ProjectId.Value);

        if (request.Status.HasValue)
            query = query.Where(a => a.Status == request.Status.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AssignedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<AllocationDto>
        {
            Items = _mapper.Map<IReadOnlyList<AllocationDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}