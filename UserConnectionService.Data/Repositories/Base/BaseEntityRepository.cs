using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Base;

namespace UserConnectionService.Data.Repositories.Base;

public class BaseEntityRepository<T>(UserMonitoringContext userMonitoringContext) : IReadWriteRepository<T>
where T : class
{
    private readonly UserMonitoringContext _userMonitoringContext = userMonitoringContext;

    public async Task AddAsync(T item) => await _userMonitoringContext
        .Set<T>()
        .AddAsync(item);

    public async Task AddRangeAsync(IEnumerable<T> items) => await _userMonitoringContext
        .Set<T>()
        .AddRangeAsync(items);

    public async Task<IEnumerable<T>> GetAsync(Func<T, bool> func) => await _userMonitoringContext
        .Set<T>()
        .AsNoTracking()
        .Where(func)
        .AsQueryable()
        .ToArrayAsync();

    public Task<long> GetCountAsync() => _userMonitoringContext
        .Set<T>()
        .AsNoTracking()
        .LongCountAsync();

    /// <summary>
    /// this one is trackable as we are going to amend its property
    /// </summary>
    public Task<T?> GetFirstOrDefaultAsync(Func<T, bool> func) => _userMonitoringContext
        .Set<T>()
        .Where(func)
        .AsQueryable()
        .FirstOrDefaultAsync();

    public Task<int> SaveAsync() => _userMonitoringContext.SaveChangesAsync();
}