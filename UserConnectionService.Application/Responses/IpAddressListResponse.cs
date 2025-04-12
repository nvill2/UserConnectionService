namespace UserConnectionService.Application.Responses;

public class IpAddressListResponse : BaseResponse
{
    public IEnumerable<string>? IpAddressList { get; set; }
}
