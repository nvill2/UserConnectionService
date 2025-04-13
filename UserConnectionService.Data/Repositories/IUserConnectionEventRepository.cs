using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Repositories;

// a repository for user connection events registering
public interface IUserConnectionEventRepository : IReadWriteRepository<UserConnectionEvent>
{
    Task<UserConnectionEvent?> GetLatestEventByUserIdAsync(long userId);

    Task<IEnumerable<string>> GetIpAddressesByUserId(long userId);

    Task<IEnumerable<long>> GetUsersWithIpStartsWith(string ipSubstring);
}
