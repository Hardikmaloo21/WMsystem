using MediatR;
using WMS.Application.Common.Exceptions;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Features.Departments.Commands;

public sealed class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IUnitOfWork _uow;

    public DeleteDepartmentCommandHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _uow.Departments.GetByIdAsync(
            request.DepartmentId,
            cancellationToken);

        if (department is null)
            throw new NotFoundException(
                nameof(Department),
                request.DepartmentId);

        await _uow.Departments.DeleteAsync(
            department,
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
    }
}