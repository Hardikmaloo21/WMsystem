
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Announcements.DTOs;
using WMS.Domain.Common;


namespace WMS.Application.Features.Announcements.Commands;

public record UpdateAnnouncementCommand(UpdateAnnouncementDto Dto)
    : IRequest<AnnouncementDto>;


public class UpdateAnnouncementCommandHandler
    : IRequestHandler<UpdateAnnouncementCommand, AnnouncementDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateAnnouncementCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AnnouncementDto> Handle(
        UpdateAnnouncementCommand request, CancellationToken ct)
    {
        var announcement = await _uow.Announcements.GetByIdAsync(
            request.Dto.AnnouncementId, ct)
            ?? throw new NotFoundException(
                nameof(Announcement), request.Dto.AnnouncementId);

        announcement.Title = request.Dto.Title;
        announcement.Message = request.Dto.Message;

        await _uow.Announcements.UpdateAsync(announcement, ct);
        await _uow.SaveChangesAsync(ct);

        var updated = await _uow.Announcements.Query()
            .Include(a => a.CreatedByEmployee)
            .FirstOrDefaultAsync(
                a => a.AnnouncementId == announcement.AnnouncementId, ct);

        return _mapper.Map<AnnouncementDto>(updated);
    }
}