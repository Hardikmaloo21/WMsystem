using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public sealed class ProjectRepository(WmsDbContext dbContext) : GenericRepository<Project>(dbContext), IProjectRepository
{
}
