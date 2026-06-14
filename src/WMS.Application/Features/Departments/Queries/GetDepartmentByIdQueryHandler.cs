using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Departments.DTOs;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Departments.Queries;

public sealed class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetDepartmentByIdQueryHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _uow.Departments
            .Query()
            .FirstOrDefaultAsync(
                d => d.DepartmentId == request.DepartmentId,
                cancellationToken);

        if (department is null)
            throw new NotFoundException(
                "Department",
                request.DepartmentId);

        return _mapper.Map<DepartmentDto>(department);
    }
}