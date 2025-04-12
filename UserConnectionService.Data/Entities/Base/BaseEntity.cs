namespace UserConnectionService.Data.Entities.Base;

public abstract class BaseEntity
{
    protected DateTimeOffset CreationDateTime { get; set; } = DateTimeOffset.UtcNow;
}
