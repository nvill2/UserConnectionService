using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;

// implementation
public class UserConnectionEventRepository : BaseEntityRepository<UserConnectionEvent>, IUserConnectionEventRepository
{
    public UserConnectionEventRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task<IEnumerable<string>> GetIpAddressesByUserId(long userId)
    {
        return await UserMonitoringContext
            .Set<UserConnectionEvent>()
            .Where(e => e.UserId == userId)
            .Distinct()
            .Select(u => u.IpAddress)            
            .ToArrayAsync();
    }

    public async Task<UserConnectionEvent?> GetLatestEventByUserIdAsync(long userId)
    {
        return await UserMonitoringContext
            .Set<UserConnectionEvent>()
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.TimeStamp)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<long>> GetUsersWithIpStartsWith(string ipSubstring)
    {
        return await UserMonitoringContext
            .Set<UserConnectionEvent>()
            .Where(e => e.IpAddress.StartsWith(ipSubstring))
            .Distinct()
            .Select(u => u.UserId)
            .ToArrayAsync();
    }
}
