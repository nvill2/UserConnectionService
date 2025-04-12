namespace UserConnectionService.Domain.Models;

public class Event
{
    public DateTimeOffset CreationDateTime { get; set; } = DateTimeOffset.UtcNow;
}
