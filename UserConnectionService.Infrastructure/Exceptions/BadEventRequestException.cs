namespace UserConnectionService.Infrastructure.Exceptions;

public class BadEventRequestException : UserConnectionServiceException
{
    public override string Message => $"The request is NULL or has wrong format."; 
}
