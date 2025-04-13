using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Repositories.Base;

public abstract class BaseEntityRepository<T> : IReadWriteRepository<T>
where T : class, IEvent
{
    protected readonly UserMonitoringContext UserMonitoringContext;

    public BaseEntityRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        UserMonitoringContext = scope.ServiceProvider.GetRequiredService<UserMonitoringContext>();
    }

    public async Task AddAsync(T item) => await UserMonitoringContext
        .Set<T>()
        .AddAsync(item);

    public async Task AddRangeAsync(IEnumerable<T> items) => await UserMonitoringContext
        .Set<T>()
        .AddRangeAsync(items);

    public IEnumerable<T> Get(Func<T, bool> func) => UserMonitoringContext
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
    public T? GetFirstOrDefault(Func<T, bool> func) => UserMonitoringContext
        .Set<T>()
        .FirstOrDefault(func);

    public Task<int> SaveAsync() => UserMonitoringContext.SaveChangesAsync();
}