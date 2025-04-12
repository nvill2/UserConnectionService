using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;

public class ErrorEventRepository : BaseEntityRepository<ErrorEvent>
{
    public ErrorEventRepository(UserMonitoringContext userMonitoringContext) : base(userMonitoringContext)
    {
    }
}
