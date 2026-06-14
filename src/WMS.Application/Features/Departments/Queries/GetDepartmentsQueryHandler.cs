using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Features.Departments.DTOs;
using WMS.Domain.Interfaces;


namespace WMS.Application.Features.Departments.Queries;

public sealed class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, IReadOnlyList<DepartmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetDepartmentsQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _uow.Departments
    .Query()
    .Include(d => d.Employees)
    .ToListAsync(cancellationToken);

        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }
}