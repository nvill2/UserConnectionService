using UserConnectionService.Application.Requests;
using UserConnectionService.Application.Responses;

namespace UserConnectionService.Application.Interfaces;

public interface IUserEventHandler
{
    Task<UserEventProcessResponse> ProcessNewEventAsync(UserEventRequest? request);

    Task<UserListResponse> GetUsersByIpStartsWithAsync(string ipAddressSubstring);

    Task<IpAddressListResponse> GetUserIpAddressesAsync(long userId);

    Task<UserEventResponse> GetUserLastConectionInfoAsync(long userId);
}
