using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public sealed class DepartmentRepository(WmsDbContext dbContext) : GenericRepository<Department>(dbContext), IDepartmentRepository
{
}
