using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;

// implementation
public class UserConnectionEventRepository : BaseEntityRepository<UserConnectionEvent>, IUserConnectionEventRepository
{
    public UserConnectionEventRepository(UserMonitoringContext userMonitoringContext) : base(userMonitoringContext)
    {
    }
}
