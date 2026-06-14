// File: WMS.Application/Features/Announcements/DTOs/AnnouncementDto.cs
namespace WMS.Application.Features.Announcements.DTOs;

public record AnnouncementDto
{
    public int AnnouncementId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public int CreatedBy { get; init; }
    public string CreatedByName { get; init; } = string.Empty;
    public DateTime CreatedOn { get; init; }
    public bool IsActive { get; init; }
}

public record CreateAnnouncementDto
{
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public int CreatedBy { get; init; }
}

public record UpdateAnnouncementDto
{
    public int AnnouncementId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}