using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;
using UserConnectionService.Data.Entities.Base;

namespace UserConnectionService.Data.Entities;

[Table("user_connection_events")]
public class UserConnectionEvent : BaseEntity
{
    public long Id { get; set; }

    public long User { get; set; }

    public string IpAddress { get; set; }
}

public class UserConnectionEventValidator : AbstractValidator<UserConnectionEvent>
{
    public UserConnectionEventValidator()
    {
        RuleFor(x => x.IpAddress).NotEmpty().Length(0, 39);
    }
}
