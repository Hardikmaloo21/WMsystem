using AutoMapper;
using MediatR;
using WMS.Application.Features.Attendance.DTOs;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Attendance.Queries;

public sealed class GetTodayAttendanceQueryHandler
    : IRequestHandler<GetTodayAttendanceQuery, object>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetTodayAttendanceQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<object> Handle(
        GetTodayAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        var today = await _uow.Attendances
    .GetTodayWithEmployeeAsync(cancellationToken);
    

        return _mapper.Map<List<AttendanceDto>>(today);
    }
}