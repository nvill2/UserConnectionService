namespace UserConnectionService.Infrastructure.Exceptions;

public class IpAddressParseException : UserConnectionServiceException
{
    public IpAddressParseException(string? ipAddress)
    {
        IpAddress = ipAddress;
    }

    public string? IpAddress { get; set; }

    public override string Message => $"Wrong IP Address format: {IpAddress}";
}
