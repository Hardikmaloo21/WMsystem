﻿// ToggleAnnouncementCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Announcements.DTOs;
using WMS.Domain.Common;


public record ToggleAnnouncementCommand(int AnnouncementId) : IRequest<AnnouncementDto>;


public class ToggleAnnouncementCommandHandler
    : IRequestHandler<ToggleAnnouncementCommand, AnnouncementDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ToggleAnnouncementCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AnnouncementDto> Handle(
        ToggleAnnouncementCommand request, CancellationToken ct)
    {
        var announcement = await _uow.Announcements.GetByIdAsync(
            request.AnnouncementId, ct)
            ?? throw new NotFoundException(
                nameof(Announcement), request.AnnouncementId);

        announcement.IsActive = !announcement.IsActive;
        await _uow.Announcements.UpdateAsync(announcement, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<AnnouncementDto>(announcement);
    }
}
