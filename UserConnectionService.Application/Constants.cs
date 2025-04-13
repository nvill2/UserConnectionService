namespace UserConnectionService.Application;

// magic strings
public static class Constants
{
    public const string SuccessResultMessage = "Success.";
    public const string FailedResultMessage = "Fail.";
    public const string EmptyOrNulRequest = "The request is null or empty.";
    public const string JobEnqueued = "A new job has been successfully enqueued.";

    public const string NoEventsForUserIdFormat = "There are no events for user, UserId = {0}.";
    public const string NotIpAddressesFormat = "There are no users connected from IP Address starting with '{0}'.";
}
