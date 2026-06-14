using MediatR;
using WMS.Application.Features.Departments.DTOs;

namespace WMS.Application.Features.Departments.Commands;

public sealed record UpdateDepartmentCommand(
    UpdateDepartmentDto Department
) : IRequest<DepartmentDto>;