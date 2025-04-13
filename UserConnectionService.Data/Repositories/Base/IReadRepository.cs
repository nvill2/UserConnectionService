namespace UserConnectionService.Data.Base;

public interface IReadRepository<T> : IRepository
    where T : class
{
    IEnumerable<T> Get(Func<T, bool> func);

    Task<long> GetCountAsync();

    T? GetFirstOrDefault(Func<T, bool> func);
}
