using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Repositories;

// a repository for user connection events registering
public interface IUserConnectionEventRepository : IReadWriteRepository<UserConnectionEvent>
{
}
