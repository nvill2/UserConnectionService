namespace UserConnectionService.Data.Base;

public interface IRepository
{
}

public interface IRepository<T> : IRepository
    where T : class
{
}
