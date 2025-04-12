using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;

// implementation
public class UserConnectionEventRepository : BaseEntityRepository<UserConnectionEvent>, IUserConnectionEventRepository
{
    public UserConnectionEventRepository(UserMonitoringContext userMonitoringContext) : base(userMonitoringContext)
    {
    }

    public async Task<UserConnectionEvent?> GetLatestEventByUserIdAsync(long userId)
    {
        return await UserMonitoringContext
            .Set<UserConnectionEvent>()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.TimeStamp)
            .FirstOrDefaultAsync();
    }
}
