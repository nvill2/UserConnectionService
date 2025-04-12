using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserConnectionService.Data.Entities;

[Table("error_events")]
public class ErrorEvent: IEvent
{
    [Key]
    public long Id { get; set; }

    public string? Data { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
}

public interface IEvent
{
}
