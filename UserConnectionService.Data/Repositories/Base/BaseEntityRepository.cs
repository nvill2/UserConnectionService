using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities.Base;

namespace UserConnectionService.Data.Repositories.Base;

public class BaseEntityRepository<T>(UserMonitoringContext userMonitoringContext) : IReadWriteRepository<T>
where T : BaseEntity
{
    private readonly UserMonitoringContext _userMonitoringContext = userMonitoringContext;

    public async Task AddAsync(T item) => await _userMonitoringContext.AddAsync(item);

    public async Task AddRangeAsync(IEnumerable<T> items) => await _userMonitoringContext.AddRangeAsync(items);

    public async Task<IEnumerable<T>> GetAsync(Func<T, bool> func) => await _userMonitoringContext.UserConenctionEvents.Where(func).AsQueryable().ToArrayAsync();

    public Task<long> GetCountAsync() => _userMonitoringContext.UserConenctionEvents.LongCountAsync();

    public Task<int> SaveAsync() => _userMonitoringContext.SaveChangesAsync();
}