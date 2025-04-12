namespace UserConnectionService.Application.Responses;

public class UserListResponse : BaseResponse
{
    public IEnumerable<long>? UserIds { get; set; }
}
