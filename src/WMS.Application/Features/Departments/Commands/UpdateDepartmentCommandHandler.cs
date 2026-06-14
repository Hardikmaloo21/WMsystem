using AutoMapper;
using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Application.Features.Departments.DTOs;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Departments.Commands;

public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateDepartmentCommandHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _uow.Departments.GetByIdAsync(
            request.Department.DepartmentId,
            cancellationToken);

        if (department is null)
            throw new NotFoundException(
                nameof(Department),
                request.Department.DepartmentId);

        department.DepartmentName =
            request.Department.DepartmentName;

        department.Description =
            request.Department.Description;

        await _uow.Departments.UpdateAsync(
            department,
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DepartmentDto>(department);
    }
}