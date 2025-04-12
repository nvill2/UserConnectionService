using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;
public class UserConnectionEventRepository : BaseEntityRepository<UserConnectionEvent>
{
    public UserConnectionEventRepository(UserMonitoringContext userMonitoringContext) : base(userMonitoringContext)
    {
    }
}
