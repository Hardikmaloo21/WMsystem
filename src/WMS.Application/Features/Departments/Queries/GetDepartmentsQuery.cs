using MediatR;
using WMS.Application.Features.Departments.DTOs;

namespace WMS.Application.Features.Departments.Queries;

public sealed record GetDepartmentsQuery()
    : IRequest<IReadOnlyList<DepartmentDto>>;