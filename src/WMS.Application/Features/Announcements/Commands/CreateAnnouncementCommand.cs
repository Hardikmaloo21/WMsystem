﻿// File: WMS.Application/Features/Announcements/Commands/CreateAnnouncementCommand.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Announcements.DTOs;
using WMS.Domain.Common;



namespace WMS.Application.Features.Announcements.Commands;

public record CreateAnnouncementCommand(CreateAnnouncementDto Dto)
    : IRequest<AnnouncementDto>;

public class CreateAnnouncementCommandHandler
    : IRequestHandler<CreateAnnouncementCommand, AnnouncementDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateAnnouncementCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<AnnouncementDto> Handle(
        CreateAnnouncementCommand request, CancellationToken ct)
    {
        var creatorExists = await _uow.Employees.ExistsAsync(
            e => e.EmployeeId == request.Dto.CreatedBy, ct);
        if (!creatorExists)
            throw new NotFoundException(nameof(Employee), request.Dto.CreatedBy);

        var announcement = _mapper.Map<Announcement>(request.Dto);
        announcement.CreatedOn = DateTime.UtcNow;
        announcement.IsActive = true;

        await _uow.Announcements.AddAsync(announcement, ct);
        await _uow.SaveChangesAsync(ct);

        var created = await _uow.Announcements.Query()
            .Include(a => a.CreatedByEmployee)
            .FirstOrDefaultAsync(
                a => a.AnnouncementId == announcement.AnnouncementId, ct);

        return _mapper.Map<AnnouncementDto>(created);
    }
}