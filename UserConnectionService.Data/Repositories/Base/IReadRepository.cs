namespace UserConnectionService.Data.Base;

public interface IReadRepository<T> : IRepository
    where T : class
{
    IEnumerable<T> GetAsync(Func<T, bool> func);

    Task<long> GetCountAsync();

    T? GetFirstOrDefaultAsync(Func<T, bool> func);
}
