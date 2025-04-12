using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace UserConnectionService.Data.Entities;

[Table("user_connection_events")]
[Index(nameof(UserId), nameof(IpAddress))]
public class UserConnectionEvent
{
    [Key]
    public long Id { get; set; }

    public long UserId { get; set; }

    public string IpAddress { get; set; }

    public long VisitCount { get; set; }

    public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
}

public class UserConnectionEventValidator : AbstractValidator<UserConnectionEvent>
{
    public UserConnectionEventValidator()
    {
        RuleFor(x => x.IpAddress).NotEmpty().Length(0, 39); // yes we have IpAddressValidator but let it be as well
    }
}
