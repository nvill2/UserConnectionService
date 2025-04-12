using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Base;

public interface IReadWriteRepository<T> : IReadRepository<T> 
    where T : class
{ 
    Task AddAsync(T item);

    Task AddRangeAsync(IEnumerable<T> items);

    Task<int> SaveAsync();
}
