using UserConnectionService.Application.Requests;
using UserConnectionService.Application.Responses;

namespace UserConnectionService.Application.Interfaces;

public interface IUserEventHandler
{
    Task<UserEventProcessResponse> ProcessEventAsync(UserEventRequest? request);

    Task<UserListResponse> GetUsersByIpStartsWith(string ipAddressSubstring);

    Task<IpAddressListResponse> GetUserIpAddresses(long userId);

    Task<UserEventResponse> GetUserLastConectionInfo(long userId);
}
