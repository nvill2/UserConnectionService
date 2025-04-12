namespace UserConnectionService.Data.Base;

public interface IReadRepository<T> : IRepository
    where T : class
{
    Task<IEnumerable<T>> GetAsync(Func<T, bool> func);

    Task<long> GetCountAsync();
}
