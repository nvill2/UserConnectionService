using UserConnectionService.Data.Base;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data.Repositories;

// a repository for storing the events that are looking like something wrong with them
public interface IErrorEventRepository : IReadWriteRepository<ErrorEvent>
{
}