using WMS.Domain.Entities;
using WMS.Domain.Interfaces.Repositories;
using WMS.Infrastructure.Persistence;

namespace WMS.Infrastructure.Repositories;

public sealed class UserLoginRepository(WmsDbContext dbContext) : GenericRepository<UserLogin>(dbContext), IUserLoginRepository
{
}
