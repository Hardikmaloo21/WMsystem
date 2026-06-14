using AutoMapper;
using MediatR;
using WMS.Application.Features.Attendance.DTOs;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Attendance.Queries;

public sealed class GetMonthlyAttendanceQueryHandler
    : IRequestHandler<GetMonthlyAttendanceQuery, IReadOnlyList<AttendanceDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetMonthlyAttendanceQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AttendanceDto>> Handle(
        GetMonthlyAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        var records = await _uow.Attendances.GetMonthlyAsync(
            request.EmployeeId,
            request.Year,
            request.Month,
            cancellationToken
            );

        return _mapper.Map<IReadOnlyList<AttendanceDto>>(records);
    }
}