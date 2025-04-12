namespace UserConnectionService.Application.Responses;

public class BaseResponse
{
    public bool IsSuccess { get; set; }

    public string? ResultMessage { get; set; }
}
