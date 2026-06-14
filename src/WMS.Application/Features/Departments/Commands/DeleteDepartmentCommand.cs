using MediatR;

namespace WMS.Application.Features.Departments.Commands;

public sealed record DeleteDepartmentCommand(
    int DepartmentId
) : IRequest;