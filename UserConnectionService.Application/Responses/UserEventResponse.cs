using UserConnectionService.Domain.Models;

namespace UserConnectionService.Application.Responses;

public class UserEventResponse : BaseResponse
{
    public UserEvent? UserEvent { get; set; }
}
