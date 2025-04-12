using System.Net;
using UserConnectionService.Infrastructure.Core.Interfaces;

namespace UserConnectionService.Infrastructure.Validators;

public class IpAddressValidator : IValidator<string>
{
    public bool Validate(string value) => IPAddress.TryParse(value, out var _);
}
