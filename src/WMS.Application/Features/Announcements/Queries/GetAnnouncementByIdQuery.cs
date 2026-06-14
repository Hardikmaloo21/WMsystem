using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Announcements.DTOs;

public record GetAnnouncementByIdQuery(int AnnouncementId)
    : IRequest<AnnouncementDto>;

public class GetAnnouncementByIdQueryHandler
    : IRequestHandler<GetAnnouncementByIdQuery, AnnouncementDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAnnouncementByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AnnouncementDto> Handle(
        GetAnnouncementByIdQuery request,
        CancellationToken ct)
    {
        var announcement = await _uow.Announcements.Query()
            .Include(a => a.CreatedByEmployee)
            .FirstOrDefaultAsync(
                a => a.AnnouncementId == request.AnnouncementId,
                ct);

        if (announcement == null)
            throw new NotFoundException(
                nameof(Announcement),
                request.AnnouncementId);

        return _mapper.Map<AnnouncementDto>(announcement);
    }
}