using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Repositories.Base;

public abstract class BaseEntityRepository<T>(UserMonitoringContext userMonitoringContext) : IReadWriteRepository<T>
where T : class, IEvent
{
    protected readonly UserMonitoringContext UserMonitoringContext = userMonitoringContext;

    public async Task AddAsync(T item) => await UserMonitoringContext
        .Set<T>()
        .AddAsync(item);

    public async Task AddRangeAsync(IEnumerable<T> items) => await UserMonitoringContext
        .Set<T>()
        .AddRangeAsync(items);

    public IEnumerable<T> GetAsync(Func<T, bool> func) => UserMonitoringContext
        .Set<T>()
        .Where(func)
        .ToArray();

    public Task<long> GetCountAsync() => UserMonitoringContext
        .Set<T>()
        .AsNoTracking()
        .LongCountAsync();

    /// <summary>
    /// this one is trackable as we are going to amend its property
    /// </summary>
    public T? GetFirstOrDefaultAsync(Func<T, bool> func) => UserMonitoringContext
        .Set<T>()
        .FirstOrDefault(func);

    public Task<int> SaveAsync() => UserMonitoringContext.SaveChangesAsync();
}