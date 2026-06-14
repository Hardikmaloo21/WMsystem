// WMS.Application/Features/Attendance/Commands/CheckOutCommand.cs
using MediatR;
using WMS.Application.Features.Attendance.DTOs;

namespace WMS.Application.Features.Attendance.Commands;

public record CheckOutCommand(CheckOutDto Dto) : IRequest<AttendanceDto>;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, AttendanceDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CheckOutCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<AttendanceDto> Handle(CheckOutCommand request, CancellationToken ct)
    {
        var attendance = await _uow.Attendances.GetTodayAttendanceAsync(request.Dto.EmpId, ct)
            ?? throw new NotFoundException("No check-in record found for today.");

        if (attendance.CheckOut.HasValue)
            throw new ConflictException("Employee has already checked out today.");

        attendance.CheckOut = DateTime.UtcNow;
        // Auto-calculate total working hours
        attendance.TotalHours = Math.Round(
            (attendance.CheckOut.Value - attendance.CheckIn).TotalHours, 2);

        await _uow.Attendances.UpdateAsync(attendance, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<AttendanceDto>(attendance);
    }
}