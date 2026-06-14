using MediatR;

namespace WMS.Application.Features.Attendance.Queries;

public sealed record GetTodayAttendanceQuery()
    : IRequest<object>;