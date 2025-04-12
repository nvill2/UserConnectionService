namespace UserConnectionService.Domain.Models;

public class UserEvent
{
    public long UserId { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset Timestamp { get; set; }
}
