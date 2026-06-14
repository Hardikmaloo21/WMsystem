// WMS.Application/Features/Employees/Queries/GetEmployeesQuery.cs
using MediatR;
using WMS.Application.Features.Employees.DTOs;
using WMS.Domain.Common;

namespace WMS.Application.Features.Employees.Queries;

public record GetEmployeesQuery(
    string? Search,
    int? DepartmentId,
    int? RoleId,
    string? Status,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<EmployeeDto>>;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PagedResult<EmployeeDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow; _mapper = mapper;
    }

    public async Task<PagedResult<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken ct)
    {
        var pagedEmployees = await _uow.Employees.SearchAsync(
            request.Search, request.DepartmentId, request.RoleId,
            request.Status, request.PageNumber, request.PageSize, ct);

        return new PagedResult<EmployeeDto>
        {
            Items = _mapper.Map<IReadOnlyList<EmployeeDto>>(pagedEmployees.Items),
            TotalCount = pagedEmployees.TotalCount,
            PageNumber = pagedEmployees.PageNumber,
            PageSize = pagedEmployees.PageSize
        };
    }
}