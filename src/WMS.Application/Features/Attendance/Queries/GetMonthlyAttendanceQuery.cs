using MediatR;
using WMS.Application.Features.Attendance.DTOs;

namespace WMS.Application.Features.Attendance.Queries;

public sealed record GetMonthlyAttendanceQuery(
    int EmployeeId,
    int Year,
    int Month
) : IRequest<IReadOnlyList<AttendanceDto>>;