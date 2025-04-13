using System.Net;
using UserConnectionService.Infrastructure.Core.Interfaces;

namespace UserConnectionService.Infrastructure.Validators;

public class IpAddressValidator : IIpAddressValidator
{
    /// <summary>
    /// Checks if the passed string has a valid IP Address format (both IPv4 and IPv6)
    /// </summary>
    public bool Validate(string value) => IPAddress.TryParse(value, out var _);
}
