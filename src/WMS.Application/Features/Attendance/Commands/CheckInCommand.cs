// WMS.Application/Features/Attendance/Commands/CheckInCommand.cs
using MediatR;
using WMS.Application.Features.Attendance.DTOs;
using AttendanceEntity = WMS.Domain.Entities.Attendance;

namespace WMS.Application.Features.Attendance.Commands;

public record CheckInCommand(CheckInDto Dto) : IRequest<AttendanceDto>;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, AttendanceDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CheckInCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<AttendanceDto> Handle(CheckInCommand request, CancellationToken ct)
    {
        // Business rule: one attendance per employee per day
        var alreadyCheckedIn = await _uow.Attendances.HasCheckedInTodayAsync(request.Dto.EmpId, ct);
        if (alreadyCheckedIn)
            throw new ConflictException("Employee has already checked in today.");

        var attendance = new WMS.Domain.Entities.Attendance
        {
            EmpId = request.Dto.EmpId,
            CheckIn = DateTime.UtcNow,
            WorkMode = request.Dto.WorkMode,
            AttendanceDate = DateTime.UtcNow.Date
        };
        new AttendanceEntity
        {
        };

        await _uow.Attendances.AddAsync(attendance, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<AttendanceDto>(attendance);
    }
}