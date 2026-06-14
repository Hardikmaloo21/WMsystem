﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Announcements.DTOs;
using WMS.Domain.Common;


public record GetAnnouncementsQuery(
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<AnnouncementDto>>;


public class GetAnnouncementsQueryHandler
    : IRequestHandler<GetAnnouncementsQuery, PagedResult<AnnouncementDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAnnouncementsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedResult<AnnouncementDto>> Handle(
        GetAnnouncementsQuery request, CancellationToken ct)
    {
        var query = _uow.Announcements.Query()
            .Include(a => a.CreatedByEmployee)
            .AsNoTracking();

        if (request.IsActive.HasValue)
            query = query.Where(a => a.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.CreatedOn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<AnnouncementDto>
        {
            Items = _mapper.Map<IReadOnlyList<AnnouncementDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
