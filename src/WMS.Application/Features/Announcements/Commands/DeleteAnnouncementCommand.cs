
﻿// DeleteAnnouncementCommand.cs
using MediatR;
using WMS.Domain.Common;

public record DeleteAnnouncementCommand(int AnnouncementId) : IRequest<Unit>;


public class DeleteAnnouncementCommandHandler
    : IRequestHandler<DeleteAnnouncementCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteAnnouncementCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(
        DeleteAnnouncementCommand request, CancellationToken ct)
    {
        var announcement = await _uow.Announcements.GetByIdAsync(
            request.AnnouncementId, ct)
            ?? throw new NotFoundException(
                nameof(Announcement), request.AnnouncementId);

        await _uow.Announcements.DeleteAsync(announcement, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
