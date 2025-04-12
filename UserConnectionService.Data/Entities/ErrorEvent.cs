using System.ComponentModel.DataAnnotations.Schema;
using UserConnectionService.Data.Entities.Base;

namespace UserConnectionService.Data.Entities;

[Table("error_events")]
public class ErrorEvent : BaseEntity
{
    public long Id { get; set; }

    public string Data { get; set; }

    public string ErrorMessage { get; set; }
}
