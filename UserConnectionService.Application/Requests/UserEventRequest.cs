namespace UserConnectionService.Application.Requests;

public class UserEventRequest
{
    public long UserId { get; set; }

    public string? IpAddress { get; set; }
}