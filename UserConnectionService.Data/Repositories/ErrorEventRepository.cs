using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories.Base;

namespace UserConnectionService.Data.Repositories;

// implementation
public class ErrorEventRepository : BaseEntityRepository<ErrorEvent>, IErrorEventRepository
{
    public ErrorEventRepository(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}
