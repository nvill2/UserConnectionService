namespace UserConnectionService.Application.Interfaces;

// this will handle our operations on background
public interface IQueuedProcessorBackgroundService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
