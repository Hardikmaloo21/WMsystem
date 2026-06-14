using AutoMapper;
using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Application.Common.Interfaces;
using WMS.Application.Features.Employees.DTOs;

namespace WMS.Application.Features.Employees.Queries;

public sealed record GetEmployeeByIdQuery(
    int EmployeeId
) : IRequest<EmployeeDto>;

public sealed class GetEmployeeByIdQueryHandler
    : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(
        GetEmployeeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _uow.Employees.GetWithDetailsAsync(
    request.EmployeeId,
    cancellationToken
    );

        if (employee is null)
            throw new NotFoundException(
                nameof(Employee),
                request.EmployeeId);

        return _mapper.Map<EmployeeDto>(employee);
    }
}